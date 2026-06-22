using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheValkyrie.TheValkyrieCode.Cards.Common;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Cards.Uncommon;
using TheValkyrie.TheValkyrieCode.Potions;

namespace TheValkyrie.TheValkyrieCode.Epochs;

public class Valkyrie3BeatAct3 : EpochModel
{
    public override string Id => "THEVALKYRIE-VALKYRIE_STORY3_BEATACT3";
    public override EpochEra Era => EpochEra.Blight2;
    public override int EraPosition => 2;
    public override string? StoryId => "CultistXMerchantYaoi";
    
    public static List<PotionModel> Potions =>
    [
        ModelDb.Potion<BottledEcho>(),
        ModelDb.Potion<EnchantedPolish>(),
        ModelDb.Potion<GlassGrenade>()
    ];

    public override string UnlockText => CreatePotionUnlockText(Potions);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueuePotionUnlock(Potions);
        NTimelineScreen.Instance.QueueMiscUnlock(new LocString("epochs", this.Id + ".unlock").GetFormattedText() ?? "");
    }
}