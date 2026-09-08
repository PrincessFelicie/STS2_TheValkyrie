using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Utilities;


namespace TheValkyrie.TheValkyrieCode.Powers;

public class CrusadePower : TheValkyriePower, IHasSecondAmount
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            if (Owner.Player != null) return [CustomHoverTipFactory.FromCombatSmiteWithEnchant(Owner.Player)];
            return [HoverTipFactory.FromCard<Smite>()];
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("BlockUpgrade", 0)
    ];
    
    public override int DisplayAmount => DynamicVars["BlockUpgrade"].IntValue;
    public string GetSecondAmount() => Amount.ToString();
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? card,
        CardPlay? cardPlay)
    {
        return Owner != dealer || !props.IsPoweredAttack() || card == null || !card.Tags.Contains(CustomEnum.Smite) ? 0 : Amount;
    }
    
    public override decimal ModifyBlockAdditive(
        Creature target,
        decimal block,
        ValueProp props,
        CardModel? card,
        CardPlay? cardPlay)
    {
        return Owner != target || !props.IsPoweredAttack() || card == null || !card.Tags.Contains(CustomEnum.Smite) ? 0 : DynamicVars["BlockUpgrade"].BaseValue;
    }
}