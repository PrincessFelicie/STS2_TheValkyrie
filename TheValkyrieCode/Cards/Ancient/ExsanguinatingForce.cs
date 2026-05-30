using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Ancient;

public class ExsanguinatingForce : TheValkyrieCard
{
    public ExsanguinatingForce() : base(1, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
        WithPower<BleedPower>(2, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<ExsanguinatingForcePower>(choiceContext, Owner.Creature, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}