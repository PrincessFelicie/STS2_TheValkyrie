using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class CutApart : TheValkyrieCard
{
    public CutApart() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithPower<BleedPower>(6, 2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}