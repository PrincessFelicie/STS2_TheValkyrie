using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Ancient;

public class MakeItDie : TheValkyrieCard
{
    public MakeItDie() : base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
    {
        WithDamage(10);
        WithPower<BleedPower>(3);
        WithVar("Hits", 3,1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        for (int i = 0; i < DynamicVars["Hits"].BaseValue; ++i)
        {
            if (play.Target != null)
            {
                await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}