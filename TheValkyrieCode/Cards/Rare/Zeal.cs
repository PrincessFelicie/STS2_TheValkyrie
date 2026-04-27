using BaseLib.Extensions;
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

public class Zeal : TheValkyrieCard
{
    public Zeal() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1, 1);
        WithEnergy(1);
        WithKeyword(CardKeyword.Exhaust);
        WithVar("Bless", 1); WithTip(CustomEnum.Bless);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        IEnumerable<CardModel> cardsDrawn = await CommonActions.Draw(this, choiceContext);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        foreach (CardModel card in cardsDrawn)
        {
            if (!card.IsUpgraded)
                CardCmd.Upgrade(card);

            if (card.Enchantment != null)
            {
                if (card.Enchantment.ShowAmount || card.Enchantment is Sown) //assume that if it's a non-show-amount enchantment (e.g. Clone, Glam, Corrupted, etc) that we can't "improve" it, so leave it be. Exception is made for Sown, which does use enchantment.amount, surprisingly
                {
                    card.Enchantment.Amount++;
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
                        enchantmentCount = 3;
                        break;
                    case Nimble:
                    case Sanguine:
                    case Adroit:
                        enchantmentCount = 2;
                        break;
                    case Aegis:
                        enchantmentCount = 1;
                        break;
                }
                if(enchantment.CanEnchant(card)) //one last check to filter out curses and statuses and the like. Sadly "canEnchant" exists on enchantment but not on cardModel, so we have to select the enchantment *before* that filter instead of not wasting compute time :/ 
                    CardCmd.Enchant(enchantment.ToMutable(), card, enchantmentCount);
            }
            
            CardCmd.Preview(card);
        }
    }

    protected override void OnUpgrade()
    {
    }
}