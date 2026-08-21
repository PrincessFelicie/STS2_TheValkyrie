using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace TheValkyrie.TheValkyrieCode.Powers;
public sealed class AngelFormPower : TheValkyriePower
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ArmorPower>()];

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player?.PlayerCombatState == null) return;
        foreach (CardModel card in Owner.Player.PlayerCombatState.AllCards.Where(c => c.Tags.Contains(CustomEnum.Smite)))
        {
            CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
            CardCmd.RemoveKeyword(card, CardKeyword.Retain); //saves up space on the card text.
            //We could consider stealing the visual of Hexed from Spectral Knight -- but not the affliction itself, since cards can only have one affliction at a time. It'd have gameplay implications.
        }

        await this.ApplyFlyTweenToOwner();
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (!card.Tags.Contains(CustomEnum.Smite) || card.Owner != this.Owner.Player)
            return Task.CompletedTask;
        CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
        CardCmd.RemoveKeyword(card, CardKeyword.Retain);
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        this.Flash();
        await PowerCmd.Apply<ArmorPower>(new ThrowingPlayerChoiceContext(), this.Owner, this.Amount, this.Owner, null);
    }

    private Task ApplyFlyTweenToOwner()
    {
        NCombatRoom? instance = NCombatRoom.Instance;
        if (TestMode.IsOn || instance == null ) return Task.CompletedTask;
        
        NCreature? ownerCreature = instance.GetCreatureNode(this.Owner);
        if (ownerCreature == null) return Task.CompletedTask;
        
        Tween tweenLiftOff = instance.CreateTween().SetParallel().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        tweenLiftOff.TweenProperty(ownerCreature, (NodePath) "global_position:y",ownerCreature.GlobalPosition.Y - 150, 0.75); //take to the skies!

        Tween tweenFloat = instance.CreateTween().SetLoops().SetParallel().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic); //looping float anim
        tweenFloat.TweenMethod(Callable.From((Action<float>) (t => ownerCreature.Visuals.Position = Vector2.Up * 10f * Mathf.Sin(t * 4f) * Mathf.Sin(t * 0.5f))), 0.0f,  6.2831855f, 3.0).SetEase(Tween.EaseType.OutIn).SetTrans(Tween.TransitionType.Sine);
        
        return Task.CompletedTask;
    }
}