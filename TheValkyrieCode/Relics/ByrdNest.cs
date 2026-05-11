using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.RestSiteOptions;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class ByrdNest : TheValkyrieRelic
{
    private int _timesHatched;
    public const int MaxHatches = 3;
    
    public override RelicRarity Rarity => RelicRarity.Rare;
    
    public override bool IsAllowed(IRunState runState)
    {
        return RelicModel.IsBeforeAct3TreasureChest(runState);
    }
    
    public override bool ShowCounter => true;
    
    public override int DisplayAmount => this.TimesHatched;
    
    [SavedProperty]
    public int TimesHatched
    {
        get => this._timesHatched;
        set
        {
            this.AssertMutable();
            this._timesHatched = value;
            this.InvokeDisplayAmountChanged();
        }
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        CustomEnum.GetStaticHoverTip("THEVALKYRIE-HATCH_FROM_NEST"),
        HoverTipFactory.FromCard<ByrdSwoop>(),
        HoverTipFactory.FromCard<Peck>(),
        HoverTipFactory.FromCard<TerritorialInstincts>(),
    ];

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != this.Owner || this.TimesHatched >= 3)
            return false;
        options.Add((RestSiteOption) new ValkyrieNestHatchRestSiteOption(player));
        return true;
    }
}