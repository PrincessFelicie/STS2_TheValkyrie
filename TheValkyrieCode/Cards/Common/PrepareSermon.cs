using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

//I do not like this card. It's too similar to Blade Dance. I'll gladly cut it if I find better.
public class PrepareSermon : TheValkyrieCard
{
    public PrepareSermon() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithKeyword(CardKeyword.Exhaust);
        WithVar("Quantity", 3);
        WithTip(typeof(Smite));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (PileType.Hand.GetPile(this.Owner).Cards.Any<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CustomEnum.Smite))))
            return;
        await Smite.CreateInHand(Owner, this.DynamicVars["Quantity"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}