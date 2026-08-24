using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class EmergencyCorrection : TheValkyrieCard
{
    public EmergencyCorrection() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(11, 4);
        WithKeyword(CardKeyword.Retain);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
    }
}