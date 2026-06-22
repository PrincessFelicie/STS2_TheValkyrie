using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheValkyrie.TheValkyrieCode.Cards.Common;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Cards.Uncommon;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Epochs;

public class Valkyrie7BeatAct2 : EpochModel
{
    public override string Id => "THEVALKYRIE-VALKYRIE_STORY7_BEATACT2";
    public override EpochEra Era => EpochEra.Invitation7;
    public override int EraPosition => 1;
    public override string? StoryId => "TrainingMontage";
    
    public static List<RelicModel> Relics =>
    [
        ModelDb.Relic<BloodiedTithe>(),
        ModelDb.Relic<AriaOfWind>(),
        ModelDb.Relic<CrudeIdol>()
    ];

    public override string UnlockText => CreateRelicUnlockText(Relics);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueRelicUnlock(Relics);
    }
}