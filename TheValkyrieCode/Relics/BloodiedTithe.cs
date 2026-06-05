using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
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

    public override Decimal ModifyPowerAmountGivenAdditive(
        PowerModel power,
        Creature giver,
        Decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        if (target == null) return 0;
        return power is not BleedPower || giver != this.Owner.Creature || target.GetPowerAmount<BleedPower>() != 0 ? 0 : DynamicVars["BleedPower"].BaseValue;
    }
    
    public override Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        this.Flash();
        return Task.CompletedTask;
    }
}