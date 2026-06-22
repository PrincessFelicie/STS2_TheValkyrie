using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheValkyrie.TheValkyrieCode.Cards.Common;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Cards.Uncommon;
using TheValkyrie.TheValkyrieCode.Potions;

namespace TheValkyrie.TheValkyrieCode.Epochs;

public class Valkyrie4KillElites : EpochModel
{
    public override string Id => "THEVALKYRIE-VALKYRIE_STORY4_KILLELITES";
    public override EpochEra Era => EpochEra.Invitation2;
    public override int EraPosition => 2;
    public override string? StoryId => "ANewChosenOne";
    
    public static List<CardModel> Cards =>
    [
        ModelDb.Card<JumpAhead>(),
        ModelDb.Card<LoseYourself>(),
        ModelDb.Card<TerritorialInstincts>()
    ];

    public override string UnlockText => CreateCardUnlockText(Cards);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCardUnlock(Cards);
    }
}