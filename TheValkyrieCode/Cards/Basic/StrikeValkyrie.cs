using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheValkyrie.TheValkyrieCode.Cards.Basic;

public class StrikeValkyrie : TheValkyrieCard
{
    public StrikeValkyrie() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(6, 3);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}