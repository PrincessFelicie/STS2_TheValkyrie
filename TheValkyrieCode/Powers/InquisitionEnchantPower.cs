using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Enchantments;


namespace TheValkyrie.TheValkyrieCode.Powers;

public class InquisitionEnchantPower : TheValkyriePower
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardGeneratedForCombat(CardModel card, bool addedByPlayer)
    {
        if (!addedByPlayer || !card.Tags.Contains<CardTag>(CustomEnum.Smite) || card.Owner.Creature != this.Owner)
            return;
        this.Flash();
        if (card.Enchantment == null)
        {
            List<EnchantmentModel> validEnchantments = new List<EnchantmentModel>();
            validEnchantments.Add(ModelDb.Enchantment<Sharp>());
            validEnchantments.Add(ModelDb.Enchantment<Nimble>());
            validEnchantments.Add(ModelDb.Enchantment<Sanguine>());
            validEnchantments.Add(ModelDb.Enchantment<Aegis>());

            EnchantmentModel enchantment = validEnchantments.TakeRandom(1, Owner.Player.RunState.Rng.CombatCardGeneration).First();
            int enchantmentCount = 1;
            switch (enchantment.CanonicalInstance)
            {
                case Sharp:
                    enchantmentCount = 3;
                    break;
                case Nimble:
                case Sanguine:
                    enchantmentCount = 2;
                    break;
                case Aegis:
                    enchantmentCount = 1;
                    break;
            }
            CardCmd.Enchant(enchantment.ToMutable(), card, enchantmentCount);
        }
        else
        {
            card.Enchantment.Amount = card.Enchantment.Amount + this.Amount;
        }
    }
}