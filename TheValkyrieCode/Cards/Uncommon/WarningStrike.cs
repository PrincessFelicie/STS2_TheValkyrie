using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class WarningStrike : TheValkyrieCard
{
    public WarningStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(3, 2);
        WithPower<VigorPower>(4, 1);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}