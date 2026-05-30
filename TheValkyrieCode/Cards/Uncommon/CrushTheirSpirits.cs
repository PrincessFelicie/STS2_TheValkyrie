using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class CrushTheirSpirits : TheValkyrieCard
{
    public CrushTheirSpirits() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("CrushTheirSpiritsPower", 1);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<CrushTheirSpiritsPower>(choiceContext, Owner.Creature, DynamicVars["CrushTheirSpiritsPower"].IntValue, Owner.Creature, this);
        if (this.IsUpgraded && CombatState != null)
        {
            foreach (Creature enemy in CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<WeakPower>(choiceContext, enemy, 1, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}