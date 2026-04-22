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

public sealed class GlassGrenade : TheValkyriePotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyEnemy;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("BleedPower", 5),
        new DynamicVar("Times", 2)
    ];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PotionModel.AssertValidForTargetedPotion(target);
        //NCombatRoom instance = NCombatRoom.Instance;
        //if (instance != null)
            //instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target)); //todo choose a vfx that's fun
        for (int i = 0; i < DynamicVars["Times"].BaseValue; ++i)
        {
            if (target != null)
            {
                await PowerCmd.Apply<BleedPower>(target, DynamicVars["BleedPower"].BaseValue, Owner.Creature, null);
            }
        }
    }
}