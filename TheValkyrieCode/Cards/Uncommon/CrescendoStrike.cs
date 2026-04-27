using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class CrescendoStrike : TheValkyrieCard
{
    public CrescendoStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9);
        WithVar("Bless", 1, 1); WithTip(CustomEnum.Bless);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(choiceContext);
    }
    
    public override Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card != this)
            return Task.CompletedTask;
        if (card.Enchantment != null)
        {
            if (card.Enchantment.ShowAmount ||
                card.Enchantment is Sown) //assume that if it's a non-show-amount enchantment (e.g. Clone, Glam, Corrupted, etc) that we can't "improve" it, so leave it be. Exception is made for Sown, which does use enchantment.amount, surprisingly
            {
                card.Enchantment.Amount += DynamicVars["Bless"].IntValue;
            }
            else
            {
                ThinkCmd.Play(new LocString("combat_messages", "CANT_IMPROVE_ENCHANT"), Owner.Creature, 2.0);
            }
        }
        else
        {
            List<EnchantmentModel> validEnchantments = new List<EnchantmentModel>();
            validEnchantments.Add(ModelDb.Enchantment<Sharp>());
            validEnchantments.Add(ModelDb.Enchantment<Adroit>());
            validEnchantments.Add(ModelDb.Enchantment<Sanguine>());
            validEnchantments.Add(ModelDb.Enchantment<Aegis>());

            
            EnchantmentModel enchantment = validEnchantments.TakeRandom(1, Owner.RunState.Rng.CombatCardGeneration).First();
            int enchantmentCount = 1;
            switch (enchantment.CanonicalInstance)
            {
                case Sharp:
                    enchantmentCount = DynamicVars["Bless"].IntValue+2;
                    break;
                case Adroit:
                case Sanguine:
                    enchantmentCount = DynamicVars["Bless"].IntValue+1;
                    break;
                case Aegis:
                    enchantmentCount = DynamicVars["Bless"].IntValue;
                    break;
            }
            CardCmd.Enchant(enchantment.ToMutable(), card, enchantmentCount);
        }
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
    }
}