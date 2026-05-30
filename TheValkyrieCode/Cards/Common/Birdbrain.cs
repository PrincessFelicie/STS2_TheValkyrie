using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class Birdbrain : TheValkyrieCard
{
    public Birdbrain() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithTip(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, null, this)).FirstOrDefault();
        if (card == null) return;
        await CardCmd.Exhaust(choiceContext, card);
        CardModel? card2 = CardFactory.GetDistinctForCombat(this.Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint), 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        if (card2 == null) return;
        if (this.IsUpgraded)
            CardCmd.Upgrade(card2);
        card2.EnergyCost.AddThisTurnOrUntilPlayed(-1, true);
        await CardPileCmd.AddGeneratedCardToCombat(card2, PileType.Hand, this.Owner);
    }

    protected override void OnUpgrade()
    {
    }
}