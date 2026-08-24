using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Powers;

public class DancingPlaguePower : TheValkyriePower
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<OverexertionPower>()];
    
    public override PowerType Type => PowerType.Buff;
    
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is OverexertionPower && applier == Owner && power.Owner == Owner && amount > 0)
        {
            this.Flash();
            for (var i = 0; i < this.Amount; ++i)
            {
                Creature? target = Owner.Player?.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
                if (target == null) return;
                await PowerCmd.Apply<OverexertionPower>(choiceContext, target, amount, null, null); //need to declare this application has no applier so CloseQuartersCombatPower doesn't double it again
            }
        }
    }
}