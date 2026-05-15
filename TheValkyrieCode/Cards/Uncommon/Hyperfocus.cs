using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Hyperfocus : TheValkyrieCard
{
    public Hyperfocus() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
        //I don't know of a way to have a HoverTipFactory or a WithTip() that runs only if the card is upgraded. Putting it in the OnUpgrade doesn't work, using if (isUpgraded) doesn't work either.
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = IsUpgraded ? new CardSelectorPrefs(SelectionScreenPromptUpgraded, 1) : new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, null, this)).FirstOrDefault();
        if (card == null) return;
        List<CardModel> list1 = PileType.Hand.GetPile(this.Owner).Cards.Except([card]).ToList();
        foreach (CardModel notCard in list1)
            await CardCmd.Exhaust(choiceContext, notCard);
        if (IsUpgraded)
            CardCmd.ApplyKeyword(card, CardKeyword.Retain);
    }

    private LocString SelectionScreenPromptUpgraded
    {
        get
        {
            LocString str = new LocString("cards", this.Id.Entry + ".selectionScreenPromptUpgraded");
            if (!str.Exists())
                throw new InvalidOperationException($"No selection screen prompt for {this.Id}.");
            this.DynamicVars.AddTo(str);
            return str;
        }
    }

    protected override void OnUpgrade()
    {
    }
}