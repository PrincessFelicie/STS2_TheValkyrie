using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Reinforce : TheValkyrieCard
{
    public Reinforce() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("Bless", 1, 1); 
        WithKeyword(CardKeyword.Exhaust);
        
        WithTip(CustomEnum.Bless);
        WithTips(c => HoverTipFactory.FromEnchantment<Aegis>(c.DynamicVars["Bless"].IntValue));
    }
    
    protected override bool ShouldGlowRedInternal => !PileType.Draw.GetPile(this.Owner).Cards.Any(c => c.Type == CardType.Skill);


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        foreach (CardModel card in (await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Draw.GetPile(this.Owner).Cards.Where(c => c.Type == CardType.Skill).OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList(), this.Owner, prefs)).ToList())
        {
            if (card.Enchantment is not Aegis)
                CardCmd.ClearEnchantment(card); //returns and does nothing if there's no enchantment, so no filter necessary
            CardCmd.Enchant<Aegis>(card, DynamicVars["Bless"].BaseValue); //Because Aegis is flagged as stackable, CardCmd.Enchant handles either creating the new enchantment or incrementing the enchantment.amount
            CardCmd.Preview(card);
        }
    }

    protected override void OnUpgrade()
    {
    }
}