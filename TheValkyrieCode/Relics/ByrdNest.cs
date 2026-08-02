using System.ComponentModel;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Extensions;
using TheValkyrie.TheValkyrieCode.RestSiteOptions;
using TheValkyrie.TheValkyrieCode.Utilities;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class ByrdNest : TheValkyrieRelic
{
    private int _timesHatched;
    private const int MaxHatches = 3;
    
    public override RelicRarity Rarity => RelicRarity.Rare;
    
    public override bool IsAllowed(IRunState runState)
    {
        return RelicModel.IsBeforeAct3TreasureChest(runState);
    }
    
    public override string PackedIconPath
    {
        get
        {
            string path;
            if (this.IsCanonical)
            {
                path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_full.png".RelicImagePath();
            }
            else
            {
                string suffix = (3 - this.TimesHatched) switch
                {
                    0 => "empty",
                    1 => "one",
                    2 => "two",
                    _ => "full"
                };

                path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_{suffix}.png".RelicImagePath();
            }
            return ResourceLoader.Exists(path) ? path : "relic.png".RelicImagePath();
        }
    }

    protected override string PackedIconOutlinePath
    {
        get
        {
            string path;
            if (this.IsCanonical)
            {
                path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_full_outline.png".RelicImagePath();
            }
            else
            {
                string suffix = (3 - this.TimesHatched) switch
                {
                    0 => "empty",
                    1 => "one",
                    2 => "two",
                    _ => "full"
                };

                path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_{suffix}_outline.png".RelicImagePath();
            }
            return ResourceLoader.Exists(path) ? path : "relic_outline.png".RelicImagePath();
        }
    }

    protected override string BigIconPath
    {
        get
        {
            string path;
            if (this.IsCanonical)
            {
                path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_full.png".BigRelicImagePath();
            }
            else
            {
                string suffix = (3 - this.TimesHatched) switch
                {
                    0 => "empty",
                    1 => "one",
                    2 => "two",
                    _ => "full"
                };

                path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_{suffix}.png".BigRelicImagePath();
            }
            return ResourceLoader.Exists(path) ? path : "relic.png".BigRelicImagePath();
        }
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
            this.RelicIconChanged(); //this only uncaches the big icon, and doesn't propagate down to the NRelic!!!
            RelicIconUpdater.ReloadRelicIcon(Id); //thank you to JohnnyBazooka89 for this piece of code!
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
        if (player != this.Owner || this.TimesHatched >= MaxHatches)
            return false;
        options.Add(new ValkyrieNestHatchRestSiteOption(player));
        return true;
    }
}