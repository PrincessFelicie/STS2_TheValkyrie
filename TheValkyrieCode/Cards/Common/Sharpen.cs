using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class Sharpen : TheValkyrieCard
{
    public Sharpen() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVar("Bless", 2, 2); WithTip(CustomEnum.Bless);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(typeof(Sanguine));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        Sanguine canonicalEnchantment = ModelDb.Enchantment<Sanguine>();
        foreach (CardModel card in (await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Draw.GetPile(this.Owner).Cards.Where(c => canonicalEnchantment.CanEnchant(c) && c.Type == CardType.Attack).OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList(), this.Owner, prefs)).ToList())
        {
            CardCmd.Enchant<Sanguine>(card, DynamicVars["Bless"].BaseValue); //Because Sanguine is flagged as stackable, CardCmd.Enchant handles either creating the new enchantment or incrementing the enchantment.amount
            CardCmd.Preview(card);
        }
    }

    protected override void OnUpgrade()
    {
    }
}