using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheValkyrie.TheValkyrieCode.Cards.Common;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Cards.Uncommon;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Epochs;

public class Valkyrie1KillBosses : EpochModel
{
    public override string Id => "THEVALKYRIE-VALKYRIE_STORY1_KILLBOSSES";
    public override EpochEra Era => EpochEra.Seeds2;
    public override int EraPosition => 2;
    public override string? StoryId => "Mazaleth";
    
    
    public static List<RelicModel> Relics =>
    [
        ModelDb.Relic<FeatheredHelmet>(),
        ModelDb.Relic<MysticMatchbox>(),
        ModelDb.Relic<ByrdNest>()
    ];

    public override string UnlockText => CreateRelicUnlockText(Relics);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueRelicUnlock(Relics);
    }
}