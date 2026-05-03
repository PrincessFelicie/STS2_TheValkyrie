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
        int amount)
    {
        if (card.Enchantment == null) //create a new enchant
        {
            List<EnchantmentModel> validEnchantments = new List<EnchantmentModel>();
            
            if (card.Type == CardType.Attack)
                validEnchantments.Add(ModelDb.Enchantment<Sharp>());
            
            if (card.TargetType is TargetType.AllEnemies or TargetType.AnyEnemy or TargetType.RandomEnemy)
                validEnchantments.Add(ModelDb.Enchantment<Sanguine>());
            
            if (card.DynamicVars.ContainsKey("Block"))
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
            if(enchantment.CanEnchant(card)) //one last check to filter out curses and statuses and the like. Sadly "canEnchant" exists on enchantment but not on cardModel, so we have to select the enchantment *before* that filter instead of not wasting compute time :/ 
                CardCmd.Enchant(enchantment.ToMutable(), card, enchantmentCount);
        }
        else //otherwise improve the current enchant
        {
            if (card.Enchantment.ShowAmount || card.Enchantment is Sown) //assume that if it's a non-show-amount enchantment (e.g. Clone, Glam, Corrupted, etc) that we can't "improve" it, so leave it be. Exception is made for Sown, which does use enchantment.amount, surprisingly
            {
                card.Enchantment.Amount++;
            }
            else
            {
                ThinkCmd.Play(new LocString("combat_messages", "CANT_IMPROVE_ENCHANT"), card.Owner.Creature, 2.0);
            }
        }
    }
}