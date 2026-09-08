using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class BlunderbussPower : TheValkyriePower
{
    private class Data
    {
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Smite>()];

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner || cardPlay.Card.Type != CardType.Attack || Owner.Player == null 
            || cardPlay.IsAutoPlay //do not trigger off autoplay cards, otherwise 2 players with blunderbuss is an infinite
            )
            return;
        this.Flash();
        CardModel card = CombatState.CreateCard<Smite>(Owner.Player).CreateDupe(Owner.Player); //making it a dupe means it won't be added to the Exhaust pile after play, but instead sent to "Limbo".
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Play, Owner.Player); //It is strange, but to trigger AfterCardGeneratedForCombat (eg Inquisition), we need to add it to a Combat Pile. Quick testing shows that this seems to work without issues, no softlock nor visual quirks -- it's just fine! Still feels like I'm doing something forbidden though.
        await CardCmd.AutoPlay(choiceContext, card, cardPlay.Target);
    }
}