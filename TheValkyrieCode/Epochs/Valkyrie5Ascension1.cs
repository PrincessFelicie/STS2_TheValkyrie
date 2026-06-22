using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheValkyrie.TheValkyrieCode.Cards.Common;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Cards.Uncommon;

namespace TheValkyrie.TheValkyrieCode.Epochs;

public class Valkyrie5Ascension1 : EpochModel
{
    public override string Id => "THEVALKYRIE-VALKYRIE_STORY5_ASCENSION1";
    public override EpochEra Era => EpochEra.Invitation5;
    public override int EraPosition => 2;
    public override string? StoryId => "RuffledFeathers";
    
    public static List<CardModel> Cards =>
    [
        ModelDb.Card<DeepBreath>(),
        ModelDb.Card<HopeInDespair>(),
        ModelDb.Card<WrittenInBlood>()
    ];
    
    public override string UnlockText => CreateCardUnlockText(Cards);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCardUnlock(Cards);
    }
}