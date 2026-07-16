using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class WeightOfResponsibility : TheValkyrieCard
{
    //wait, doesn't the new version go crazy with Smites in the exhaust pile? After testing, it *is* a lot of investment since you still need to play the song cards to generate the Smites.
    public WeightOfResponsibility() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVar("DisplayExtraDamage", 2, 1);
        WithCalculatedDamage(9, 2, 
            (card, _) =>
            {
                if (card.Owner.PlayerCombatState != null)
                    return card.Owner.PlayerCombatState.AllCards.Sum(c =>
                        c.Enchantment != null ? 1 : 0);
                return 0;
            },
            ValueProp.Move,0, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}