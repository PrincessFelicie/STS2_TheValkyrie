using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class PressYourAdvantage : TheValkyrieCard
{
    public PressYourAdvantage() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<WeakPower>(2);
        WithCalculatedBlock(0, 1,
            (card, _) =>
            {
                decimal count = PileType.Hand.GetPile(card.Owner).Cards.Count;
                CardPile? pile = card.Pile;
                if (pile is { Type: PileType.Hand })
                    --count;
                return count;
            },
            ValueProp.Move, 3, 0);
        WithVar("DisplayBlockPerCard", 1, 0);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null) return;
        await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars["WeakPower"].IntValue, Owner.Creature, this);
        await CommonActions.CardBlock(this, DynamicVars.CalculatedBlock, play);
    }

    protected override void OnUpgrade()
    {
    }
}