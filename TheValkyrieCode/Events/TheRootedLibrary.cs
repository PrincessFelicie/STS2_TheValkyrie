using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Runs;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Extensions;
using TheValkyrie.TheValkyrieCode.ModConfiguration;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Events;

public class TheRootedLibrary : CustomEventModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new StringVar("Enchantment", ModelDb.Enchantment<PerfectedFlay>().Title.GetFormattedText()),
        new StringVar("Relic", ModelDb.Relic<RousingDitty>().Title.GetFormattedText()),
        new ("PerfectedFlayAmount", 2),
        new HealVar(0M)
    ];
    
    public override void CalculateVars()
    {
        DynamicVars.Heal.BaseValue = Owner != null ? HealRestSiteOption.GetHealAmount(Owner) : 0M;
    }

    public override string CustomInitialPortraitPath => "/events/the_rooted_library.png".ImagePath();
    
    public override string CustomBackgroundScenePath => SceneHelper.GetScenePath("events/background_scenes/" + ModelDb.Event<SelfHelpBook>().Id.Entry.ToLowerInvariant());
    
    protected override IReadOnlyList<EventOption> GenerateInitialOptions() 
    {
        List<EventOption> initialOptions = [];
        if (Owner == null) return initialOptions;
        if (Owner.Deck.Cards.Any(c => DeckFilter(c, ModelDb.Enchantment<PerfectedFlay>())))
            initialOptions.Add(
                Option(StudyWeakpoints,
                HoverTipFactory.FromEnchantment<PerfectedFlay>(DynamicVars["PerfectedFlayAmount"].IntValue)
                    .Append(HoverTipFactory.FromCard<Writhe>()))
                );
        else
        {
            initialOptions.Add(
                LockedOption("STUDY_WEAKPOINTS_LOCKED")
                );
        }
        initialOptions.Add(Option(NabPoetry, HoverTipFactory.FromRelic<RousingDitty>()));
        initialOptions.Add(Option(DozeOff, [HoverTipFactory.FromCard<PoorSleep>()]));
        return initialOptions;
    }

    public override bool IsAllowed(IRunState runState)
    {
        if (!ValkyrieModConfig.EventRootedLibrary) return false;
        return runState.CurrentActIndex == 1 //must be on act 2
               && runState.Players.All(
            p => p.Deck.Cards.Any(
                c => DeckFilter(c, ModelDb.Enchantment<PerfectedFlay>()
                ))); //every player needs a multi-hit attack
    }
    
    private bool DeckFilter(CardModel? card, EnchantmentModel enchantment)
    {
        return card?.Pile?.Type == PileType.Deck && enchantment.CanEnchant(card);
    }

    public override ActModel[] Acts =>
    [
        ModelDb.Act<Hive>()
    ];
    
    private async Task StudyWeakpoints()
    {
        await SelectAndEnchant<PerfectedFlay>(DynamicVars["PerfectedFlayAmount"].IntValue);
        if (Owner != null) await CardPileCmd.AddCurseToDeck<Writhe>(Owner);
        SetEventFinished(L10NLookup("THEVALKYRIE-THE_ROOTED_LIBRARY.pages.STUDY_WEAKPOINTS.description"));
    }
    
    private async Task NabPoetry()
    {
        if (Owner != null) await RelicCmd.Obtain<RousingDitty>(Owner);
        SetEventFinished(L10NLookup("THEVALKYRIE-THE_ROOTED_LIBRARY.pages.NAB_POETRY.description"));
    }
    
    private async Task DozeOff()
    {
        if (Owner == null) return;
        
        await PlayerCmd.MimicRestSiteHeal(Owner, false);
        if (LocalContext.IsMe(Owner))
        {
            int restHandle = NDebugAudioManager.Instance.Play("sleep.tres", 0.8f);
            await Cmd.CustomScaledWait(0.7f, 1.5f);
            NDebugAudioManager.Instance.Stop(restHandle);
            SfxCmd.Play("event:/sfx/enemy/enemy_attacks/waterfall_giant/waterfall_giant_knockout");
            NGame.Instance?.ScreenRumble(ShakeStrength.Medium, ShakeDuration.Normal, RumbleStyle.Rumble);
        }

        await CardPileCmd.AddCurseToDeck<PoorSleep>(Owner);

        SetEventFinished(L10NLookup("THEVALKYRIE-THE_ROOTED_LIBRARY.pages.DOZE_OFF.description"));
    }
    
    private async Task SelectAndEnchant<T>(
        int amount)
        where T : EnchantmentModel
    {
        CardSelectorPrefs prefs = new (CardSelectorPrefs.EnchantSelectionPrompt, 1);
        if (Owner == null) return;
        
        CardModel? card = (await CardSelectCmd.FromDeckForEnchantment(Owner, ModelDb.Enchantment<T>(), amount, c => DeckFilter(c, ModelDb.Enchantment<PerfectedFlay>()), prefs)).FirstOrDefault();
        if (card != null) await ApplyEnchantment<T>(card, amount);
        
    }
    
    private Task ApplyEnchantment<T>(CardModel card, int amount) where T : EnchantmentModel
    {
        CardCmd.Enchant<T>(card, amount);
        NCardEnchantVfx? child = NCardEnchantVfx.Create(card);
        if (child == null) return Task.CompletedTask;
        NRun? instance = NRun.Instance;
        instance?.GlobalUi.CardPreviewContainer.AddChildSafely(child);
        return Task.CompletedTask;
    }
}