using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class EagerJab : TheValkyrieCard
{
    public EagerJab() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(3, 2);
    }
    
    protected override bool ShouldGlowGoldInternal => Owner.HasPower<VigorPower>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int hitCount = Owner.HasPower<VigorPower>() ? 2 : 1;
        await CommonActions.CardAttack(this, play.Target, hitCount).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}