using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class Birdbrain : TheValkyrieCard
{
    public Birdbrain() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithUpgradingCardTip<ByrdSwoop>();
    }
    
    //protected override bool ShouldGlowRedInternal => !PileType.Hand.GetPile(Owner).Cards.Except([this]).Any();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //remains from a previous version, in case I want to roll it back.
        /*CardSelectorPrefs prefs = new (CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        CardModel? exhaustedCard = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (exhaustedCard == null) return;
        await CardCmd.Exhaust(choiceContext, exhaustedCard);*/
        
        if (CombatState == null) return;
        CardModel createdCard = CombatState.CreateCard<ByrdSwoop>(Owner);
        if (this.IsUpgraded)
            CardCmd.Upgrade(createdCard);
        await CardPileCmd.AddGeneratedCardToCombat(createdCard, PileType.Discard, Owner);
        CardCmd.Preview(createdCard);
    }

    protected override void OnUpgrade()
    {
    }
}