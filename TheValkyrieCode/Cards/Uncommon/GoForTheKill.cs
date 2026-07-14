using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class GoForTheKill : TheValkyrieCard
{
    public GoForTheKill() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(5, 2);
        WithTip(typeof(BleedPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null) return;
        AttackCommand attackCommand = await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, 
            attackCommand.Results.SelectMany(r => r).Sum(r => r.UnblockedDamage), 
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}