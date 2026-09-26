using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class RainOfFeathers : TheValkyrieCard
{
    public RainOfFeathers() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithPower<BleedPower>(1, 1);
        WithDamage(1, 1);
        WithVar("Hits", 2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).WithHitCount(this.DynamicVars["Hits"].IntValue).BeforeDamage((Func<Task>) (async () =>
        {
            if (CombatState == null) return;
            await PowerCmd.Apply<BleedPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["BleedPower"].BaseValue, Owner.Creature,this);
        })).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}