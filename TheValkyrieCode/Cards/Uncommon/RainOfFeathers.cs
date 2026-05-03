using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards;
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
        for (int i = 0; i < this.DynamicVars["Hits"].BaseValue; ++i)
        {
            await CommonActions.CardAttack(this, play).Execute(choiceContext);
            if (CombatState == null) return;
            await PowerCmd.Apply<BleedPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["BleedPower"].BaseValue, Owner.Creature, null);
        }
    }

    protected override void OnUpgrade()
    {
    }
}