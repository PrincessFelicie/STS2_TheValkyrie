using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;


namespace TheValkyrie.TheValkyrieCode.Powers;

public class InquisitionEnchantPower : TheValkyriePower
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

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || !card.Tags.Contains(CustomEnum.Smite) || card.Owner.Creature != this.Owner || Owner.Player == null)
            return;
        this.Flash();

        await BlessCmd.EnchantOrUpgradeEnchant(card, this.Amount);
    }
}