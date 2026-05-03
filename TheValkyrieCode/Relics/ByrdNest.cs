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
        //HoverTipFactory.Static(CustomEnum.HatchFromNest) //I swear I do not know how to make static tips work
        //HoverTipFactory.FromCard<ByrdSwoop>(), //will keep the cards a surprise until the static tooltip works
        //HoverTipFactory.FromCard<Peck>(), //todo fix me
        //HoverTipFactory.FromCard<TerritorialPurpose>(),
    ];

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != this.Owner || this.TimesHatched >= 3)
            return false;
        options.Add((RestSiteOption) new ValkyrieNestHatchRestSiteOption(player));
        return true;
    }
}