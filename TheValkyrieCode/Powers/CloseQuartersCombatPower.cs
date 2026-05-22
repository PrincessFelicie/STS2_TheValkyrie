using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;

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
        await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), this.Owner, this.Amount, this.Owner, null);
        
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
}