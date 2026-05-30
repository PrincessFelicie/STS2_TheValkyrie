using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class IcarusComplexPower : TheValkyriePower
{
    private class Data
    {
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override decimal ModifyPowerAmountGiven(
        PowerModel power,
        Creature giver,
        decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        return power is VigorPower && giver == Owner ? amount * (((decimal) Amount + 100) / 100) : amount;
    }
}