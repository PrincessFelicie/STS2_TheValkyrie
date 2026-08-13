using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class CrudeIdol : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [
            HoverTipFactory.FromPower<OverexertionPower>()
        ];

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature) || Owner.Creature.GetPowerAmount<OverexertionPower>() == 0)
            return;
        this.Flash();
        await Cmd.Wait(0.1f);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, -Math.Ceiling((decimal) Owner.Creature.GetPowerAmount<OverexertionPower>()/2), Owner.Creature, null);
    }
}