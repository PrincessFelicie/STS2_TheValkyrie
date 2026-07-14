using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class TelegraphedSmash : TheValkyrieCard
{
    public TelegraphedSmash() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVar("PerCardDamage", 2);
        WithCalculatedDamage(1, 2, 
             (card, _) =>
            {
                decimal count = PileType.Hand.GetPile(card.Owner).Cards.Count;
                CardPile? pile = card.Pile;
                if (pile is { Type: PileType.Hand })
                    --count;
                return count;
            },
        ValueProp.Move, 4);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}