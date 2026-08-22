using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class RisingPunch : TheValkyrieCard
{
    public RisingPunch() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(4, 1);
        WithVar("Times", 3);
        WithPower<OverexertionPower>(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, DynamicVars["Times"].IntValue).Execute(choiceContext);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}