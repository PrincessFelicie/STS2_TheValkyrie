using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Enchantments;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Sanctify : TheValkyrieCard
{
    public Sanctify() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Bless", 1);
        WithKeyword(CardKeyword.Exhaust);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        
        WithTip(CustomEnum.Bless);
        WithTips(c => HoverTipFactory.FromEnchantment<Refrain>(c.DynamicVars["Bless"].IntValue));
    }
    
    protected override bool ShouldGlowRedInternal => !PileType.Draw.GetPile(this.Owner).Cards.Any(c => c.Type is not (CardType.Curse or CardType.Quest or CardType.Status));


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        foreach (CardModel card in (await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Draw.GetPile(this.Owner).Cards.Where(c => c.Type is not (CardType.Curse or CardType.Quest or CardType.Status)).OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList(), this.Owner, prefs)).ToList())
        {
            if (card.Enchantment is not Refrain)
                CardCmd.ClearEnchantment(card); //returns and does nothing if there's no enchantment, so no filter necessary
            CardCmd.Enchant<Refrain>(card, DynamicVars["Bless"].BaseValue); //Because Refrain is flagged as stackable, CardCmd.Enchant handles either creating the new enchantment or incrementing the enchantment.amount
            CardCmd.Preview(card);
        }
    }

    protected override void OnUpgrade()
    {
    }
}