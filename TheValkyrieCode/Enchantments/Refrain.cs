using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Enchantments;

public class Refrain : CustomEnchantmentModel
{
    protected override string CustomIconPath => "TheValkyrie/images/enchantments/refrain.png";
    
    public override bool CanEnchant(CardModel c)
    {
        return base.CanEnchant(c);
    }
    
    public override bool ShowAmount => true;
    
    public override int EnchantPlayCount(int originalPlayCount)
    {
        return originalPlayCount + this.Amount;
    }
}