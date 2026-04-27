using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class BurstOfPower : TheValkyrieCard
{
    public BurstOfPower() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Bless", 1, 1); WithTip(CustomEnum.Bless);
        WithPower<OverexertionPower>(10);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        foreach (CardModel card in PileType.Hand.GetPile(this.Owner).Cards)
        {
            if (card.Enchantment != null)
            {
                if (card.Enchantment.ShowAmount || card.Enchantment is Sown) //assume that if it's a non-show-amount enchantment (e.g. Clone, Glam, Corrupted, etc) that we can't "improve" it, so leave it be. Exception is made for Sown, which does use enchantment.amount, surprisingly
                {
                    card.Enchantment.Amount += DynamicVars["EnchantmentAmount"].IntValue;
                }
                else
                {
                    ThinkCmd.Play(new LocString("combat_messages", "CANT_IMPROVE_ENCHANT"), Owner.Creature, 2.0);
                }
            }
            else
            {
                List<EnchantmentModel> validEnchantments = new List<EnchantmentModel>();
                validEnchantments.Add(ModelDb.Enchantment<Aegis>()); //always valid
                if (card.Type == CardType.Attack)
                    validEnchantments.Add(ModelDb.Enchantment<Sharp>());
                if (card.Type == CardType.Attack && card.TargetType != TargetType.RandomEnemy)
                    validEnchantments.Add(ModelDb.Enchantment<Sanguine>());
                if (card.DynamicVars.ContainsKey("Block"))
                    validEnchantments.Add(ModelDb.Enchantment<Nimble>());
                else
                    validEnchantments.Add(ModelDb.Enchantment<Adroit>());
                
                
                EnchantmentModel enchantment = validEnchantments.TakeRandom(1, Owner.RunState.Rng.CombatCardGeneration).First();
                int enchantmentCount = 1;
                switch (enchantment.CanonicalInstance)
                {
                    case Sharp:
                        enchantmentCount = 2 + DynamicVars["EnchantmentAmount"].IntValue;
                        break;
                    case Nimble:
                    case Sanguine:
                    case Adroit:
                        enchantmentCount = 1 + DynamicVars["EnchantmentAmount"].IntValue;
                        break;
                    case Aegis:
                        enchantmentCount = 0 + DynamicVars["EnchantmentAmount"].IntValue;
                        break;
                }
                if(enchantment.CanEnchant(card)) //one last check to filter out curses and statuses and the like. Sadly "canEnchant" exists on enchantment but not on cardModel, so we have to select the enchantment *before* that filter instead of not wasting compute time :/ 
                    CardCmd.Enchant(enchantment.ToMutable(), card, enchantmentCount);
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}