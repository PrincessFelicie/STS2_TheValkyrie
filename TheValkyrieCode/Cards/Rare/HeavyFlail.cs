using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class HeavyFlail : TheValkyrieCard
{
    public HeavyFlail() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(9, 4);
        WithTip(typeof(VigorPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        AttackCommand attackCommand = await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, 
            attackCommand.Results.SelectMany(r => r).Sum(r => r.TotalDamage + r.OverkillDamage), 
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}