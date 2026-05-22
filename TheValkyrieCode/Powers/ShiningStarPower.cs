using BaseLib.Extensions;
using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
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
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != this.Owner || result.UnblockedDamage <= 0) 
            return;
        this.DynamicVars["IsActive"].BaseValue = 0;
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Side || this.DynamicVars["IsActive"].BaseValue == 1) //at the start of our turn, reset the power...
            return;
        this.DynamicVars["IsActive"].BaseValue = 1;
        this.Flash();
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side || this.DynamicVars["IsActive"].BaseValue == 0) //at the end of the enemy turn, check if we had a perfect turn...
            return;
        await Cmd.CustomScaledWait(0.2f, 0.6f);
        this.Flash();
        //A fun vfx for this would be a mini grand-finale where a spotlight shines on the Valkyrie, then clapping sounds play
        NDebugAudioManager.Instance?.Play("hey.mp3"); //placeholder sound
        await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), this.Owner, this.Amount, this.Owner, null);
    }
}