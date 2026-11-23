using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class Birdbrain : TheValkyrieCard
{
    public Birdbrain() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithUpgradingCardTip<ByrdSwoop>();
    }
    
    //protected override bool ShouldGlowRedInternal => !PileType.Hand.GetPile(Owner).Cards.Except([this]).Any();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //remains from a previous version, in case I want to roll it back. Exhausted another card to turn into a random card
        /*CardSelectorPrefs prefs = new (CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        CardModel? exhaustedCard = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (exhaustedCard == null) return;
        await CardCmd.Exhaust(choiceContext, exhaustedCard);*/
        
        if (CombatState == null) return;
        CardModel createdCard = CombatState.CreateCard<ByrdSwoop>(Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(createdCard);
        if (Enchantment != null)
        {
            EnchantmentModel enchantment = Enchantment.CanonicalInstance;
            CardCmd.Enchant(enchantment.ToMutable(), createdCard, Enchantment.Amount);
        }
        if (!play.IsLastInSeries)
            return;
        TransformIntoAfterPlay = createdCard;
    }

    protected override void OnUpgrade()
    {
    }
}