using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Potions;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Potions;

public sealed class BottledEcho : TheValkyriePotion
{
    public override PotionRarity Rarity => PotionRarity.Common;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyEnemy;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(30, ValueProp.Unpowered),
        new DynamicVar("OverexertionPower", 10)
    ];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PotionModel.AssertValidForTargetedPotion(target);
        DamageVar damage = DynamicVars.Damage;
        /*NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
            instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target));*/ //todo choose a vfx that's fun
        await CreatureCmd.Damage(choiceContext, target, damage.BaseValue, damage.Props, Owner.Creature, null);
        await PowerCmd.Apply<OverexertionPower>(Owner.Creature, DynamicVars["OverexertionPower"].BaseValue, Owner.Creature, null);
    }
}