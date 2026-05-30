using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class ChurchHymnPower : TheValkyriePower
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

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner.Player)
            return;
        this.Flash();
        await Smite.CreateInHand(Owner.Player, this.Amount, combatState);
        
    }

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        await PowerCmd.Apply<OverexertionPower>(choiceContext, this.Owner, Amount*3, this.Owner, null, true);
    }
}