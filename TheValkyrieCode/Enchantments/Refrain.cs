using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Enchantments;

public class Refrain : CustomEnchantmentModel
{
    protected override string CustomIconPath => "TheValkyrie/images/enchantments/refrain.png";
    
    public override bool CanEnchant(CardModel c)
    {
        return base.CanEnchant(c);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
    
    public override bool IsStackable => true;
    
    public override bool ShowAmount => true;
    
    public override int EnchantPlayCount(int originalPlayCount)
    {
        return originalPlayCount + this.Amount;
    }
}