using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class LoseYourself : TheValkyrieCard
{
    protected override bool ShouldGlowGoldInternal => this.Owner.Creature.GetPowerAmount<OverexertionPower>() >= 10;

    public LoseYourself() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<BleedPower>(6, 3);
        WithPower<OverexertionPower>(10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        if (this.Owner.Creature.GetPowerAmount<OverexertionPower>() >= 10)
        {
            await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
        }
        else
        {
            await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
    }
}