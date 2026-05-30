using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;

namespace TheValkyrie.TheValkyrieCode.Cards.Basic;


public class DefendValkyrie : TheValkyrieCard
{
    public DefendValkyrie() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithBlock(5, 3);
        WithTags(CardTag.Defend);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
    }
}