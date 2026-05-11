using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.CustomRunModifiers;

public class BirdUp : ModifierModel
{
    protected override string IconPath => "TheValkyrie/images/enchantments/sanguine.png"; //todo placeholder

    public override Func<Task> GenerateNeowOption(EventModel eventModel)
    {
        return (Func<Task>) (() => OfferRewards(eventModel.Owner ?? throw new InvalidOperationException()));
    }
    
    private static async Task OfferRewards(Player player)
    {
        List<CardPileAddResult> results = new List<CardPileAddResult>();
        results.Add(await CardPileCmd.Add(player.RunState.CreateCard<Greed>(player), PileType.Deck));
        results.Add(await CardPileCmd.Add(player.RunState.CreateCard<ByrdonisEgg>(player), PileType.Deck));
        results.Add(await CardPileCmd.Add(player.RunState.CreateCard<Greed>(player), PileType.Deck));
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