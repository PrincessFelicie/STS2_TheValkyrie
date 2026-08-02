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
        WithDamage(4);
        WithVar("Times", 2, 1);
        WithTip(typeof(VigorPower));
    }
    
    protected override bool ShouldGlowGoldInternal => Owner.HasPower<VigorPower>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int hitCount = Owner.HasPower<VigorPower>() ? DynamicVars["Times"].IntValue : 1;
        await CommonActions.CardAttack(this, play, hitCount).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}