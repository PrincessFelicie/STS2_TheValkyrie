using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;


namespace TheValkyrie.TheValkyrieCode.Powers;

public class CrusadePower : TheValkyriePower
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? card)
    {
        return this.Owner != dealer || !props.IsPoweredAttack() || card == null || !card.Tags.Contains<CardTag>(CustomEnum.Smite) ? 0 : this.Amount;
    }
    
    public override Decimal ModifyBlockAdditive(
        Creature target,
        Decimal block,
        ValueProp props,
        CardModel? card,
        CardPlay? cardPlay)
    {
        return this.Owner != target || !props.IsPoweredAttack() || card == null || !card.Tags.Contains<CardTag>(CustomEnum.Smite) ? 0 : this.Amount;
    }
}