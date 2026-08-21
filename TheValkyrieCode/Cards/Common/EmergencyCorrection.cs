using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class EmergencyCorrection : TheValkyrieCard
{
    public EmergencyCorrection() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(11, 3);
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