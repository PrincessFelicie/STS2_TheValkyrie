using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class Bullrush : TheValkyrieCard
{
    public Bullrush() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(26, 6);
        WithPower<OverexertionPower>(10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AttackCommand attackCommand = await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        if (!attackCommand.Results.SelectMany(r => r).Any(r => r.WasTargetKilled))
            await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}