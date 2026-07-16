using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class PlumedWish : TheValkyrieCard
{
    public PlumedWish() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithPower<OverexertionPower>(8);
        WithKeyword(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new (SelectionScreenPrompt, 1);
        List<CardModel> validPiles = PileType.Draw.GetPile(Owner).Cards.OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList().Concat(PileType.Discard.GetPile(Owner).Cards.OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList()).ToList(); //I'm not 100% happy with this because you can't intuit that it's the draw followed by the discard. I wish it was possible to make it clearer where one pile ends and the other begins.
        CardModel? card = (await CardSelectCmd.FromSimpleGrid(choiceContext, validPiles, Owner, prefs)).FirstOrDefault();
        if (card == null)
            return;
        await CardPileCmd.Add(card, PileType.Hand);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}