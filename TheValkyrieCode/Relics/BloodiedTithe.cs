using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class BloodiedTithe : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<BleedPower>(3),
    ];

    public override Decimal ModifyPowerAmountGiven(
        PowerModel power,
        Creature giver,
        Decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        if (target == null) //this is necessary otherwise the game crashes. ModifyPowerAmountGiven is called pre-emptively to dynamically modify amounts on cards, so if there's no target yet and you try to get the bleed amount on a target, it crashes. So we must check for target null first.
            return amount;
        return !(power is BleedPower) || giver != this.Owner.Creature || target.GetPowerAmount<BleedPower>() != 0 ? amount : amount + DynamicVars["BleedPower"].BaseValue;
    }
    
    public override Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        this.Flash();
        return Task.CompletedTask;
    }
}