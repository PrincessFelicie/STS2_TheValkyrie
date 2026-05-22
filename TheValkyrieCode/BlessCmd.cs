using BaseLib.Extensions;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Enchantments;

namespace TheValkyrie.TheValkyrieCode;

public class BlessCmd
{
    public static async Task EnchantOrUpgradeEnchant(
        CardModel card,
        int amount = 1)
    {
        if (!CanBless(card)) //if we can't bless the card, return early
        {
            if (card.Enchantment != null) //if there is an enchantment but it's of a kind we can't bless, let the character have a thought bubble about it
                ThinkCmd.Play(new LocString("combat_messages", "CANT_IMPROVE_ENCHANT"), card.Owner.Creature, 2.0);
            return;
        }
        
        if (card.Enchantment == null) //create a new enchant
        {
            List<EnchantmentModel> validEnchantments = new List<EnchantmentModel>();
            
            if (card.Type == CardType.Attack)
                validEnchantments.Add(ModelDb.Enchantment<Sharp>());
            
            if (card.TargetType is TargetType.AllEnemies or TargetType.AnyEnemy or TargetType.RandomEnemy)
                validEnchantments.Add(ModelDb.Enchantment<Sanguine>());
            
            if (card.GainsBlock)
                validEnchantments.Add(ModelDb.Enchantment<Nimble>());
            else
                validEnchantments.Add(ModelDb.Enchantment<Adroit>());
            
            validEnchantments.Add(ModelDb.Enchantment<Aegis>());

            
            EnchantmentModel enchantment = validEnchantments.TakeRandom(1, card.Owner.RunState.Rng.CombatCardGeneration).First();
            int enchantmentCount = 1;
            switch (enchantment.CanonicalInstance)
            {
                case Sharp:
                    enchantmentCount = amount+2;
                    break;
                case Nimble:
                case Sanguine:
                case Adroit:
                    enchantmentCount = amount+1;
                    break;
                case Aegis:
                    enchantmentCount = amount;
                    break;
            }
            CardCmd.Enchant(enchantment.ToMutable(), card, enchantmentCount);
        }
        else //otherwise improve the current enchant
        {
            card.Enchantment.Amount+=amount;
            card.Enchantment.RecalculateValues(); //do this otherwise Adroit causes multiplayer state divergences
        }
    }

    public static bool CanBless(
        CardModel card)
    {
        return (card.Enchantment == null || card.Enchantment.ShowAmount) && card.Type is not (CardType.Curse or CardType.Status or CardType.Quest);
    }
}