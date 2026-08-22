using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class CloseQuartersCombatPower : TheValkyriePower, IHasSecondAmount
{
    public float InitialPositionFirstPlayerX = 0;
    private float _offsetDistanceFromFirstPlayerX = 0;
    
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("OverexertMult", 1)
    ];
    
    public string GetSecondAmount() => DynamicVars["OverexertMult"].IntValue.ToString();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<OverexertionPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        Creature? firstPositionPlayerCreature = LocalContext.GetMe(CombatState)?.Creature; //local player is always placed in "player 1" position, so we can use its position as a reference point from which to slide other players
        NCreature? firstPositionPlayerCreatureNode =  firstPositionPlayerCreature?.GetCreatureNode();
        NCreature? ownerPlayerCreatureNode =  NCombatRoom.Instance?.GetCreatureNode(Owner);
        
        if (firstPositionPlayerCreature == null || firstPositionPlayerCreatureNode == null ||
            ownerPlayerCreatureNode == null) return;

        if (firstPositionPlayerCreature == Owner || !firstPositionPlayerCreature.HasPower<CloseQuartersCombatPower>()) //if the current person is the one obtaining it OR they do not have it...
        {
            InitialPositionFirstPlayerX = firstPositionPlayerCreatureNode.GlobalPosition.X; //Their position hasn't changed: save it
        }
        else //otherwise, they already had it from before...
        {
            InitialPositionFirstPlayerX = firstPositionPlayerCreature.GetPower<CloseQuartersCombatPower>()!.InitialPositionFirstPlayerX; //their position has changed since: grab the one they'd saved at the time
        }
        
        _offsetDistanceFromFirstPlayerX = InitialPositionFirstPlayerX - ownerPlayerCreatureNode.GlobalPosition.X;
        
        await this.VisualsMoveToLeftMostEnemy();
    }

    public override async Task AfterCreatureAddedToCombat(Creature creature)
    {
        await this.VisualsMoveToLeftMostEnemy(); //if a new creature is spawned (e.g. mid-fight minions, like w/ ovicopter), we want the Owner of CQC to move accordingly to avoid hiding any creature
    }

    public override async Task AfterDeath(
        PlayerChoiceContext choiceContext,
        Creature creature,
        bool wasRemovalPrevented,
        float deathAnimLength)
    {
        await this.VisualsMoveToLeftMostEnemy(); //if a creature dies, recalculate where to move in case it was the leftmost one
    }

    public override async Task AfterCurrentHpChanged(Creature creature, Decimal delta)
    {
        if (creature.Side != Owner.Side && delta >= 0)
            await this.VisualsMoveToLeftMostEnemy(); //this last check is for enemy creatures reviving, like illusions or decamillipede... there's no on revive hook, so we'll just run anytime a creature on the opponent side is healed for any reason, since revives are secretly a normal heal
    }

    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        return !props.IsPoweredAttack() || cardSource == null || cardSource.Owner.Creature != this.Owner || target == null ? 1 : 1 + (decimal) this.Amount / 100;
    }
    
    public override decimal ModifyPowerAmountGivenMultiplicative(
        PowerModel power,
        Creature giver,
        decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        return power is OverexertionPower && giver == Owner && target == Owner && amount > 0 ? DynamicVars["OverexertMult"].BaseValue : 1;
    }

    private async Task VisualsMoveToLeftMostEnemy()
    {
        NCombatRoom? instance = NCombatRoom.Instance;
        if (TestMode.IsOn || instance == null || CombatState.ContainsMonster<TheInsatiable>() || Owner.HasPower<SurroundedPower>()) //Insatiable and Surrounded fights are exceptions
            return;
        
        NCreature? ownerCreature = instance.GetCreatureNode(this.Owner);
        NCreature? targetCreature = null;
        
        foreach (Creature creature in CombatState.GetOpponentsOf(Owner)) //Find the left-most enemy...
        {
            if (creature.IsDead) continue;
            targetCreature ??= instance.GetCreatureNode(creature);
            if (instance.GetCreatureNode(creature)?.GlobalPosition.X < targetCreature?.GlobalPosition.X)
            {
                    targetCreature = instance.GetCreatureNode(creature);
            }
        }

        if (targetCreature == null) return; //if there's no more creatures alive do nothing
        
        Tween tween = instance.CreateTween().SetParallel().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(ownerCreature, (NodePath) "global_position:x",targetCreature.GlobalPosition.X - Math.Max(targetCreature.Hitbox.Size.X * 1, 300) - (_offsetDistanceFromFirstPlayerX * 0.5), 0.75); //place the CQC user at the left edge of the leftmost creature's hitbox, plus an extra 75% of the creature's size for padding. In multiplayer, add half the offset as padding 
    }
}