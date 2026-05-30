using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;


namespace TheValkyrie.TheValkyrieCode.Powers;

public class InquisitionUpgradePower : TheValkyriePower
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || !card.Tags.Contains<CardTag>(CustomEnum.Smite) || card.Owner.Creature != this.Owner)
            return;
        this.Flash();
        CardCmd.Upgrade(card);
    }
}