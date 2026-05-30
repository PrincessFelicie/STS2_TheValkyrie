using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class HopeInDespairPower : TheValkyriePower
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

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        return card.Owner.Creature != this.Owner ? playCount : playCount + this.Amount;
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        await PowerCmd.Remove(this);
    }
}