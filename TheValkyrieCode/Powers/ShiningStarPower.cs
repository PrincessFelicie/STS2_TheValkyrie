using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class ShiningStarPower : TheValkyriePower
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
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0) 
            return;
        DynamicVars["IsActive"].BaseValue = 0;
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner) || DynamicVars["IsActive"].BaseValue == 1) //at the start of our turn, reset the power...
            return;
        DynamicVars["IsActive"].BaseValue = 1;
        Flash();
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner) || DynamicVars["IsActive"].BaseValue == 0) //at the end of the enemy turn, check if we had a perfect turn...
            return;
        await Cmd.CustomScaledWait(0.2f, 0.6f);
        Flash();
        //A fun vfx for this would be a mini grand-finale where a spotlight shines on the Valkyrie, then clapping sounds play
        NDebugAudioManager.Instance?.Play("hey.mp3", 1F, PitchVariance.Medium); //placeholder sound
        await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
    }
}