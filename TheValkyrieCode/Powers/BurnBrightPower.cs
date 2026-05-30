using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class BurnBrightPower : TheValkyriePower
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

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player != this.Owner.Player ? amount : amount + this.Amount;
    }
}