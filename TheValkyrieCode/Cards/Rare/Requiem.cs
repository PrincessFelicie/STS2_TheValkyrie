using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Requiem : TheValkyrieCard
{
    public Requiem() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithCalculatedVar("CalculatedSmites",0, (card, _) => PileType.Exhaust.GetPile(card.Owner).Cards.Count(c => c.Tags.Contains(CustomEnum.Smite)));
        WithCalculatedVar("CalculatedEnchantedSmites",0, (card, _) => PileType.Exhaust.GetPile(card.Owner).Cards.Count(c => c.Tags.Contains(CustomEnum.Smite) && c.Enchantment != null));
        WithTip(typeof(Smite));
        WithKeyword(CardKeyword.Exhaust);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null) return;
        IEnumerable<CardModel> list = PileType.Exhaust.GetPile(Owner).Cards.Where(c => c.Tags.Contains(CustomEnum.Smite)).ToList();
        foreach (CardModel card in list)
        {
            await CardCmd.AutoPlay(choiceContext, card, play.Target, skipCardPileVisuals: true);
        }
    }
}