using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Hyperfocus : TheValkyrieCard
{
    public Hyperfocus() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new (SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, c => !c.Keywords.Contains(CardKeyword.Retain), this)).FirstOrDefault(); 
        
        if (card == null) return; //if no card was valid, then either the hand was empty, or it only had retain cards. Either scenario we don't need to do anything for, so return.
        
        CardCmd.ApplyKeyword(card, CardKeyword.Retain);
        
        foreach (CardModel notCard in PileType.Hand.GetPile(this.Owner).Cards.Except([card]).ToList() //For each card in the player's hand except the selected one...
                     .Where(notCard => !notCard.Keywords.Contains(CardKeyword.Retain))) //if they do not have Retain...
        {
            await CardCmd.Exhaust(choiceContext, notCard); //Exhaust them
        }
    }

    protected override void OnUpgrade()
    {
    }
}