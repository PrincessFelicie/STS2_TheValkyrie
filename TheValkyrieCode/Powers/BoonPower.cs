using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;


namespace TheValkyrie.TheValkyrieCode.Powers;

public class BoonPower : TheValkyriePower
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

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        this.Flash();
        for (int i = 0; i < this.Amount; ++i)
        {
            var player = this.Owner.Player;
            if (player == null) continue;
            CardModel card = PileType.Hand.GetPile(player).Cards.Where(BlessCmd.CanBless).TakeRandom(1, player.RunState.Rng.CombatCardSelection).First();
            await BlessCmd.EnchantOrUpgradeEnchant(card);
        }
    }
}