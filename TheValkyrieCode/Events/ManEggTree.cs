using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;
using TheValkyrie.TheValkyrieCode.Extensions;
using TheValkyrie.TheValkyrieCode.ModConfiguration;

namespace TheValkyrie.TheValkyrieCode.Events;

public class ManEggTree: CustomEventModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(10)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
    [
        Option(Approach),
        Option(Leave)
    ];

    public override bool IsAllowed(IRunState runState)
    {
        return ValkyrieModConfig.FunnyContent && this.Rng.NextInt(0, 100) == 66;
    }

    public override ActModel[] Acts =>
    [
        ModelDb.Act<Hive>()
    ];
    
    private async Task Approach()
    {
        EventOption yes = Option(Yes, "APPROACH");
        EventOption no = Option(No, "APPROACH");
        this.SetEventState(this.L10NLookup("THEVALKYRIE-MAN_EGG_TREE.pages.APPROACH.description"), [yes, no]);
    }
    
    private async Task Leave()
    {
        this.SetEventFinished(this.L10NLookup("THEVALKYRIE-MAN_EGG_TREE.pages.LEAVE.description"));
    }
    
    private async Task Yes()
    {
        if (this.Owner == null) return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(this.Owner.RunState.CreateCard<ByrdonisEgg>(this.Owner), PileType.Deck), 2f);
        this.SetEventFinished(this.L10NLookup("THEVALKYRIE-MAN_EGG_TREE.pages.YES.description"));
    }
    
    private async Task No()
    {
        this.SetEventFinished(this.L10NLookup("THEVALKYRIE-MAN_EGG_TREE.pages.NO.description"));
    }
    
    public override string CustomInitialPortraitPath => "/events/man_egg_tree.png".ImagePath();
    public override string CustomBackgroundScenePath => SceneHelper.GetScenePath("events/background_scenes/" + ModelDb.Event<ThisOrThat>().Id.Entry.ToLowerInvariant());
}