using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Extensions;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Potions;

public sealed class EnchantedPolish : TheValkyriePotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyPlayer;
    
    public override string CustomPackedImagePath => "/potions/enchanted_polish.png".ImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("ArmorPower", 3)
    ];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PotionModel.AssertValidForTargetedPotion(target);
        //NCombatRoom instance = NCombatRoom.Instance;
        //if (instance != null)
            //instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target)); //todo choose a vfx that's fun
        await PowerCmd.Apply<ArmorPower>(choiceContext, target, DynamicVars["ArmorPower"].BaseValue, Owner.Creature, null);
    }
}