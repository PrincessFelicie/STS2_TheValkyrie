using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class PerfectYourStance : TheValkyrieCard
{
    public PerfectYourStance() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(12, 5);
        WithPower<ArmorPower>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AttackCommand attackCommand = await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        if (attackCommand.Results.SelectMany(r => r).Sum(r => r.UnblockedDamage) > 0)
        {
            await PowerCmd.Apply<ArmorPower>(choiceContext, Owner.Creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
    }
}