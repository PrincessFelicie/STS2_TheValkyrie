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
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Event;

//Sad that this card doesn't work
/*public class Swoop : TheValkyrieCard
{
    public Swoop() : base(2, CardType.Attack, CardRarity.Event, TargetType.AnyEnemy)
    {
        WithDamage(14, 4);
        WithTips(c => [HoverTipFactory.FromCard<ByrdSwoop>(c.IsUpgraded)]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null) return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(choiceContext);
        if (CombatState == null) return;
        CardModel byrdSwoop = CombatState.CreateCard<ByrdSwoop>(this.Owner);
        if (this.IsUpgraded)
            CardCmd.Upgrade(byrdSwoop);
        if (this.Enchantment != null)
        {
            EnchantmentModel enchantment = this.Enchantment.ToMutable();
            CardCmd.Enchant(enchantment, byrdSwoop, this.Enchantment.Amount);
        }
        await CardCmd.Transform(this, byrdSwoop); //no!!! it does not like that! it gets stuck in the play pile!!!
        await CardPileCmd.Add(this, this.GetResultPileType()); //doesn't work!!! replacing this with byrdSwoop doesn't work either!!! oh noes!!! there's no hook late enough to handle this without getting Pilestuck!!! Only solution is to Harmony Patch in a brand new post-discard hook!!! Unfortunate!!!!
    }

    protected override void OnUpgrade()
    {
    }
}*/