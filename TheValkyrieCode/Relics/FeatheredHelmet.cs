using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class FeatheredHelmet : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ArmorPower>(1)];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        this.Flash();
        await PowerCmd.Apply<ArmorPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["ArmorPower"].BaseValue, Owner.Creature, null);
    }
}