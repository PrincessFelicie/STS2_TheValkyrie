using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Requiem : TheValkyrieCard
{
    public Requiem() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        CardModel card = this;
        WithCalculatedVar("CalculatedSmites",0, (card, _) => (decimal) PileType.Exhaust.GetPile(card.Owner).Cards.Count((Func<CardModel, bool>) (c => c.Tags.Contains(CustomEnum.Smite))));
        WithCalculatedVar("CalculatedEnchantedSmites",0, (card, _) => (decimal) PileType.Exhaust.GetPile(card.Owner).Cards.Count((Func<CardModel, bool>) (c => c.Tags.Contains(CustomEnum.Smite) && c.Enchantment != null)));
        WithTip(typeof(Smite));
        WithKeyword(CardKeyword.Exhaust);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Requiem requiem = this;
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        IEnumerable<CardModel> list = PileType.Exhaust.GetPile(requiem.Owner).Cards.Where((Func<CardModel, bool>) (c => c.Tags.Contains(CustomEnum.Smite))).ToList();
        bool flag = true;
        foreach (CardModel card in list)
        {
            await CardCmd.AutoPlay(choiceContext, card, play.Target, skipCardPileVisuals: !flag);
            flag = false;
        }
    }
}