using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class BloodiedTithe : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<BleedPower>(3),
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
    [
        HoverTipFactory.FromPower<BleedPower>()
    ];

    public override Decimal ModifyPowerAmountGivenAdditive(
        PowerModel power,
        Creature giver,
        Decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        return power is not BleedPower || giver != this.Owner.Creature || target == null || target.GetPowerAmount<BleedPower>() != 0 ? 0 : DynamicVars["BleedPower"].BaseValue;
    }
    
    public override Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        if (power.Owner != null) //this check necessary because, despite being *supposedly* non-nullable, Owner can still be null if artifact blocked the application of the power
            this.Flash([power.Owner]);
        return Task.CompletedTask;
    }
}