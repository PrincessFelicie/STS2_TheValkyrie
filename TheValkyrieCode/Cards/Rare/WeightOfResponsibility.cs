using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class WeightOfResponsibility : TheValkyrieCard
{
    public WeightOfResponsibility() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVar("DisplayExtraDamage", 1, 1);
        WithCalculatedDamage(9, 1, 
            (card, _) =>
            {
                if (card.Owner.PlayerCombatState != null)
                    return card.Owner.PlayerCombatState.AllCards.Sum(c =>
                        c.Enchantment != null ? BlessCmd.CanBless(c) ? c.Enchantment.Amount : 2 : 0);
                return 0;
            },
            ValueProp.Move,0, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}