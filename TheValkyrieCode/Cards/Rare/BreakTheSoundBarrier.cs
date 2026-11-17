using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class BreakTheSoundBarrier : TheValkyrieCard
{
    public BreakTheSoundBarrier() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(5);
        WithVar(new HitCountVar(5).WithUpgrade(1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, DynamicVars.HitCount.IntValue).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}