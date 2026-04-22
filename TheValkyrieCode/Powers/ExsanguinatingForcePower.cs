using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Powers;
public sealed class ExsanguinatingForcePower : TheValkyriePower
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side)
            return;
        this.Flash();
        //await Cmd.CustomScaledWait(0.2f, 0.4f);
        /*foreach (Creature hittableEnemy in (IEnumerable<Creature>) CombatState.HittableEnemies)
        {
            NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(hittableEnemy);
            if (creatureNode != null)
                NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Node) NGaseousImpactVfx.Create(creatureNode.VfxSpawnPosition, new Color("83eb85")));
        }*/
        await PowerCmd.Apply<BleedPower>((IEnumerable<Creature>) CombatState.HittableEnemies, this.Amount, this.Owner, null);
    }
}