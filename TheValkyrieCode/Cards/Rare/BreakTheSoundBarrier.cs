using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class BreakTheSoundBarrier : TheValkyrieCard
{
    public BreakTheSoundBarrier() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(5);
        WithVar("Times", 5, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, DynamicVars["Times"].IntValue).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}