using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Cacophony : TheValkyrieCard
{
    public Cacophony() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        CardModel card = this;
        WithCalculatedDamage(0, ( card, _) => card.Owner.Creature.GetPowerAmount<OverexertionPower>());
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        WithTip(typeof(OverexertionPower)); //We do want the tooltip for the Bleed mechanic though
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await PowerCmd.Remove(Owner.Creature.GetPower<OverexertionPower>());
    }

    protected override void OnUpgrade()
    {
    }
}