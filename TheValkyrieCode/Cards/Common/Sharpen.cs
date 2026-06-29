using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class Sharpen : TheValkyrieCard
{
    public Sharpen() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVar("Bless", 3, 2);
        WithKeyword(CardKeyword.Exhaust);

        WithTip(typeof(BleedPower));
        WithTip(CustomEnum.Bless);
        WithTips(c => HoverTipFactory.FromEnchantment<Sanguine>(c.DynamicVars["Bless"].IntValue));
    }

    protected override bool ShouldGlowRedInternal => PileType.Draw.GetPile(this.Owner).Cards.All(c => c.Type != CardType.Attack);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        foreach (CardModel card in (await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Draw.GetPile(this.Owner).Cards.Where(c => c.Type == CardType.Attack).OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList(), this.Owner, prefs)).ToList())
        {
            if (card.Enchantment is not Sanguine)
                CardCmd.ClearEnchantment(card); //returns and does nothing if there's no enchantment, so no filter necessary
            CardCmd.Enchant<Sanguine>(card, DynamicVars["Bless"].BaseValue); //Because Sanguine is flagged as stackable, CardCmd.Enchant handles either creating the new enchantment or incrementing the enchantment.amount
            CardCmd.Preview(card);
        }
    }

    protected override void OnUpgrade()
    {
    }
}