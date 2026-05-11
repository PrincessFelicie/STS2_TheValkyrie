using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Powers;
public sealed class AngelFormPower : TheValkyriePower
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
    
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player?.PlayerCombatState == null) return Task.CompletedTask;
        foreach (CardModel card in Owner.Player.PlayerCombatState.AllCards.Where(c => c.Tags.Contains(CustomEnum.Smite)))
        {
            CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
            CardCmd.RemoveKeyword(card, CardKeyword.Retain); //saves up space on the card text.
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (!card.Tags.Contains(CustomEnum.Smite) || card.Owner != this.Owner.Player)
            return Task.CompletedTask;
        CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
        CardCmd.RemoveKeyword(card, CardKeyword.Retain);
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        this.Flash();
        await PowerCmd.Apply<ArmorPower>(new ThrowingPlayerChoiceContext(), this.Owner, this.Amount, this.Owner, null);
    }
}