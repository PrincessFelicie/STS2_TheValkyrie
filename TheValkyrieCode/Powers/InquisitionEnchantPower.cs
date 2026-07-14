using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards.Token;


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
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Smite>(), CustomEnum.GetStaticHoverTip("Bless")];
    
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