using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class WrittenInBloodPower : TheValkyriePower
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
    
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != this.Owner || result.UnblockedDamage <= 0)
        {
            return;
        }
        await PowerCmd.Remove(this);
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        var ownerPlayer = this.Owner.Player;
        if (ownerPlayer != null)
            for (int i = 0; i < this.Amount; ++i)
            {
                room.AddExtraReward(ownerPlayer, new RelicReward(ModelDb.Relic<ALyricsSheet>().ToMutable(), ownerPlayer));
            }
        return Task.CompletedTask;
    }
}