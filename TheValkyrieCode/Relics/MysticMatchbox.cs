using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class MysticMatchbox : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(2, ValueProp.Unpowered),
        new BlockVar(1, ValueProp.Unpowered)
    ];

    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        return !props.IsPoweredAttack() || cardSource?.Enchantment == null || cardSource.Owner != this.Owner ? 0 : (Decimal) this.DynamicVars.Damage.IntValue;
    }
    
    public override Decimal ModifyBlockAdditive(
        Creature target,
        Decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        return this.Owner.Creature != target || cardSource?.Enchantment == null || cardSource.Owner != this.Owner ? 0 : (Decimal) this.DynamicVars.Block.IntValue;
    }
}