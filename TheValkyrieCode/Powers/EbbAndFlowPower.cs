using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class EbbAndFlowPower : TheValkyriePower
{
    private class Data
    {
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BoolVar("IsActive", true)
    ];

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<OverexertionPower>()];
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not OverexertionPower) return;
        if (Owner.GetPowerAmount<OverexertionPower>() >= 15 && this.DynamicVars["IsActive"].BaseValue == 1)
        {
            this.Flash();
            this.DynamicVars["IsActive"].BaseValue = 0;
            await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, Owner, Amount, Owner, null);
        }
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner) || this.DynamicVars["IsActive"].BaseValue == 1) //at the start of our turn, reset the power...
            return;
        this.DynamicVars["IsActive"].BaseValue = 1;
        this.Flash();
    }
}