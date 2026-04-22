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
        WithPower<BleedPower>(2, 2);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) (card => card.Type == CardType.Attack && card.TargetType != TargetType.RandomEnemy && card.Enchantment == null), (AbstractModel) this)).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        CardCmd.Enchant<Sanguine>(card, DynamicVars["BleedPower"].BaseValue);
    }

    protected override void OnUpgrade()
    {
    }
}