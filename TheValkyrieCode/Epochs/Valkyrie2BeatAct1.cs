using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheValkyrie.TheValkyrieCode.Cards.Common;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Cards.Uncommon;

namespace TheValkyrie.TheValkyrieCode.Epochs;

public class Valkyrie2BeatAct1 : EpochModel
{
    public override string Id => "THEVALKYRIE-VALKYRIE_STORY2_BEATACT1";
    public override EpochEra Era => EpochEra.Blight1;
    public override int EraPosition => 2;
    public override string? StoryId => "BirthOfTheFlock";
    
    public static List<CardModel> Cards =>
    [
        ModelDb.Card<Shriek>(),
        ModelDb.Card<RustedDagger>(),
        ModelDb.Card<PlumedWish>()
    ];
    
    public override string UnlockText => CreateCardUnlockText(Cards);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCardUnlock(Cards);
    }
}