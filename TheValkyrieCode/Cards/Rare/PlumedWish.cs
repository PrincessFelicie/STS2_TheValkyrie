using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
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
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        List<CardModel> validPiles = PileType.Draw.GetPile(this.Owner).Cards.OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList().Concat(PileType.Discard.GetPile(this.Owner).Cards.OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList()).Concat(PileType.Exhaust.GetPile(this.Owner).Cards.OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList()).ToList();
        CardModel? card = (await CardSelectCmd.FromSimpleGrid(choiceContext, validPiles, this.Owner, prefs)).FirstOrDefault();
        if (card == null)
            return;
        CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}