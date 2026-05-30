using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class Tenderize : TheValkyrieCard
{
    public Tenderize() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithPower<BleedPower>(3, 1);
        WithPower<VulnerablePower>(1, 1);
    }

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, DynamicVars["VulnerablePower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}