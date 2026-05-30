using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Study : TheValkyrieCard
{
    public Study() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(9, 4);
        WithCards(3);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.Draw(this, choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}