using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class BleedPower : TheValkyriePower, IHasSecondAmount
{
    private class Data
    {
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Decay", 0)
    ];

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => DynamicVars["Decay"].IntValue;
    public string GetSecondAmount() => Amount.ToString();

    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(Amount, Color.FromHtml("#890000"), HealthBarForecastDirection.FromRight, 0, null, AffectsHpLabel:false)];
    }
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this && amount > 0)
        {
            await Cmd.Wait(0.1f);
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), power.Owner, power.Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
            DynamicVars["Decay"].BaseValue++;
        }
        this.InvokeSecondAmountChanged();
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        if (Owner.IsAlive)
        {
            await PowerCmd.Apply<BleedPower>(new ThrowingPlayerChoiceContext(), Owner, -DynamicVars["Decay"].IntValue,
                Owner, null);
            DynamicVars["Decay"].BaseValue = 0;
            this.InvokeSecondAmountChanged();
        }
    }
}