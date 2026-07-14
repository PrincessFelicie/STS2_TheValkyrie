using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class CloseQuartersCombatPower : TheValkyriePower
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BoolVar("IsUpgraded", false)
    ];
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        this.Flash();
        if (this.DynamicVars["IsUpgraded"].BaseValue == 1)
        {
            foreach (Creature enemy in CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), enemy, this.Amount, Owner, null);
            }
        }
        else
        {
            if (Owner.Player == null) return;
            await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), CombatState.HittableEnemies.TakeRandom(1, Owner.Player.RunState.Rng.CombatTargets).First(), this.Amount, Owner, null);
        }
    }
    
    public override decimal ModifyPowerAmountGivenMultiplicative(
        PowerModel power,
        Creature giver,
        decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        return power is OverexertionPower && giver == Owner && amount > 0 ? 2 : 1;
    }
}