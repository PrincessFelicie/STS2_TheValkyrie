using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Utilities;


namespace TheValkyrie.TheValkyrieCode.Powers;

public class InquisitionPower : TheValkyriePower, IHasSecondAmount
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Bless", 0)
    ];

    public string GetSecondAmount() => DynamicVars["Bless"].IntValue == 0 ? "" : DynamicVars["Bless"].IntValue.ToString();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            IEnumerable<IHoverTip> hoverTips = [];
            
            if (Owner.Player != null)
                hoverTips = hoverTips.Append(CustomHoverTipFactory.FromCombatSmiteWithEnchant(Owner.Player));
            else hoverTips = hoverTips.Append(HoverTipFactory.FromCard<Smite>());
            
            if(DynamicVars["Bless"].IntValue > 0) hoverTips = hoverTips.Append(HoverTipFactory.FromKeyword(CustomEnum.Bless));
            
            return hoverTips;
        }
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || !card.Tags.Contains(CustomEnum.Smite) || card.Owner.Creature != this.Owner || Owner.Player == null)
            return;
        this.Flash();
        
        CardCmd.Upgrade(card);

        await BlessCmd.EnchantOrUpgradeEnchant(card, DynamicVars["Bless"].IntValue);
    }
}