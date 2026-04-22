using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class ArmorPower : TheValkyriePower
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

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (Owner == dealer || dealer == null || target != Owner) return 0;
        if (props.IsPoweredAttack_() && dealer.IsEnemy) //this means the power won't work if given to enemies, which limits design space a bit (not that I think giving enemies negative armor would be in flavor for this character, but still). I tried replacing isEnemy with dealer.side != target.side, but it just caused the game to crash if armor was given to the enemy. Aaaaaaah welllll)
        {
            return -Amount;
        }
        return 0;
    }
}