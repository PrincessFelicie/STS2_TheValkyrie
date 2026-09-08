using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Utilities;

public class CustomHoverTipFactory
{
    public static IHoverTip FromCardWithEnchant(CardModel card, bool upgrade, EnchantmentModel enchantment, int enchantAmount = 1)
    {
        card = (CardModel) card.MutableClone();
        enchantment = (EnchantmentModel) enchantment.MutableClone();
        if (enchantAmount != 0)
        {
            card.EnchantInternal(enchantment, enchantAmount);
            enchantment.ModifyCard();
        }
        switch (enchantment)
        {
            case Sharp:
                card.DynamicVars.Damage.UpgradeValueBy(enchantAmount);
                //card.DynamicVars.CalculatedDamage.UpgradeValueBy(enchantAmount); fix me if/when need arises
                break;
            case Nimble:
                card.DynamicVars.Block.UpgradeValueBy(enchantAmount);
                //card.DynamicVars.CalculatedBlock.UpgradeValueBy(enchantAmount);
                break;
        }
        if (upgrade)
        {
            card.UpgradeInternal();
        }
        card.FinalizeUpgradeInternal();
        
        return new CardHoverTip(card);
    }
    
    //this is awful code and I don't care. I'm working within the limitations of the base game here :( 
    public static IHoverTip FromCombatSmiteWithEnchant(Player owner, EnchantmentModel? enchantment = null, int enchantAmount = 0)
    {
        CardModel card = (CardModel) ModelDb.Card<Smite>().MutableClone();
        
        enchantment ??= ModelDb.Enchantment<RandomBlessing>();
        enchantment = (EnchantmentModel) enchantment.MutableClone();
        
        if (owner.HasPower<InquisitionPower>())
        {
            enchantAmount += owner.Creature.GetPower<InquisitionPower>().DynamicVars["Bless"].IntValue;
            
            card.UpgradeInternal();
            card.FinalizeUpgradeInternal();
        }
        
        if (enchantAmount != 0)
        {
            card.EnchantInternal(enchantment, enchantAmount);
            enchantment.ModifyCard();
        }

        switch (enchantment)
        {
            case Sharp:
                card.DynamicVars.Damage.UpgradeValueBy(enchantAmount); //I really shouldn't be needing to do this... fix these enchants Megacrit :(
                break;
            case Nimble:
                card.DynamicVars.Block.UpgradeValueBy(enchantAmount);
                break;
        }

        if (owner.GetRelic<BookOfOrnithology>() != null) //all of these hooks that upgrade dynamicvars in combat should really improve the damage in hovertip previews too. free improvement to the base game ripe for picking
        {
            card.DynamicVars.Damage.UpgradeValueBy(
                owner.Relics.Count(r => r is BookOfOrnithology) *
                owner.GetRelic<BookOfOrnithology>().DynamicVars["BonusDamage"].BaseValue);
        }

        if (owner.HasPower<CrusadePower>()) //seriously, accuracy, phantom blades and blade of ink are right there. Why can't the shiv previews update as well? Imagine how banger it'd be Megacrit. Or heck hire me I'll do it
        {
            card.DynamicVars.Damage.UpgradeValueBy(owner.Creature.GetPowerAmount<CrusadePower>());
            card.DynamicVars.Block.UpgradeValueBy(owner.Creature.GetPower<CrusadePower>().DynamicVars["BlockUpgrade"].BaseValue);
        }
        
        //hate having to individually account for every source of modifications to my tokens when it ought to be automated
        
        return new CardHoverTip(card);
    }
}