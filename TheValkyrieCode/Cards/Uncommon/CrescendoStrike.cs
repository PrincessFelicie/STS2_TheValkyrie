using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

//This card does not compare nicely to Kingly Punch... Option A is to raise its bless-per-draw amount, option B is to change how blessings stack...
public class CrescendoStrike : TheValkyrieCard
{
    public CrescendoStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9);
        WithVar("Bless", 1, 1); WithTip(CustomEnum.Bless);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card != this)
            return;
        await BlessCmd.EnchantOrUpgradeEnchant(card, DynamicVars["Bless"].IntValue);
    }

    protected override void OnUpgrade()
    {
    }
}