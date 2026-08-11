using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.CustomRunModifiers;

public class BirdUp : ModifierModel
{
    protected override string IconPath => "TheValkyrie/images/relics/big/byrd_nest_full.png"; //todo placeholder

    public override Func<Task> GenerateNeowOption(EventModel eventModel)
    {
        return (Func<Task>) (() => OfferRewards(eventModel.Owner ?? throw new InvalidOperationException()));
    }
    
    private static async Task OfferRewards(Player player)
    {
        List<CardPileAddResult> results = new List<CardPileAddResult>();
        results.Add(await CardPileCmd.Add(player.RunState.CreateCard<ByrdonisEgg>(player), PileType.Deck));
        results.Add(await CardPileCmd.Add(player.RunState.CreateCard<ByrdonisEgg>(player), PileType.Deck));
        results.Add(await CardPileCmd.Add(player.RunState.CreateCard<ByrdonisEgg>(player), PileType.Deck));
        CardCmd.PreviewCardPileAdd(results, 2);
        await RelicCmd.Obtain<ByrdNest>(player);
        await Cmd.CustomScaledWait(0.6f, 1.2f);
    }
}

[HarmonyPatch(typeof(ModelDb))]
public static class GoodModifiersPatch
{
    [HarmonyPatch("GoodModifiers", MethodType.Getter)]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    public static void Postfix(ref IEnumerable<ModifierModel> __result)
    {
        __result = __result.Append(ModelDb.Modifier<BirdUp>());
    }
}

// code below snatched and adapted from Cany0udance's Curated Challenges 2! Thank you for your contributions to the modding scene.
[HarmonyPatch(typeof(Byrdpip), nameof(Byrdpip.AfterObtained))]
public static class ByrdpipAfterObtainedPatch
{
    static bool Prefix(Byrdpip __instance, ref Task __result)
    {
        if (!AreWeInChallengeBirdUp())
            return true; // Use original behavior
        
        // Use modified behavior
        __result = TransformSingleEgg(__instance);
        return false; // Then skip original method
    }
    
    private static async Task TransformSingleEgg(Byrdpip byrdpip)
    {
        byrdpip.Skin = new Rng((uint)(byrdpip.Owner.NetId + byrdpip.Owner.RunState.Rng.Seed)).NextItem(Byrdpip.SkinOptions);
        
        // Find first egg in deck
        CardModel? egg = PileType.Deck.GetPile(byrdpip.Owner).Cards.FirstOrDefault(c => c is ByrdonisEgg);
        
        if (egg != null)
        {
            await CardCmd.TransformTo<ByrdSwoop>(egg);
        }
        
        // Summon pet if in combat
        if (CombatManager.Instance.IsInProgress)
        {
            await PlayerCmd.AddPet<MegaCrit.Sts2.Core.Models.Monsters.Byrdpip>(byrdpip.Owner);
        }
    }
    
    private static bool AreWeInChallengeBirdUp()
    {
        if (!RunManager.Instance.IsInProgress) return false;
        
        RunState? runState = RunManager.Instance.DebugOnlyGetState(); // todo replace this with publicizing RunManager.State if/when this gets deprecated
        if (runState == null) return false;
        
        return CheckModifierIDsForMatch(ModelDb.Modifier<BirdUp>());
    }
    
    public static bool CheckModifierIDsForMatch(ModifierModel modifier)
    {
        var readOnlyList = RunManager.Instance.DebugOnlyGetState()?.Modifiers;
        return readOnlyList != null && readOnlyList.Any(runModifiers => modifier.Id == runModifiers.Id);
    }
}

//fixes a bug in base game where having the egg multiple times will have the Hatch option show up multiple times. Since this modifier makes that way easier to run into, might as well patch it... Once again, thank you to Cany0udance!
[HarmonyPatch(typeof(ByrdonisEgg), nameof(ByrdonisEgg.TryModifyRestSiteOptions))]
public static class ByrdonisEggRestSiteOptionsPatch
{
    static bool Prefix(ByrdonisEgg __instance, Player player, ICollection<RestSiteOption> options, ref bool __result)
    {
        if (player != __instance.Owner)
        {
            __result = false;
            return false;
        }
        
        // Only add the Hatch option if it's not already present
        if (options.All(opt => opt.OptionId != "HATCH"))
        {
            options.Add(new HatchRestSiteOption(player));
        }
        
        __result = true;
        return false; // Skip original method
    }
}