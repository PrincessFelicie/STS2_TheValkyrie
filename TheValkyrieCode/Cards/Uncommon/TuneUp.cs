using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class TuneUp : TheValkyrieCard
{
    public TuneUp() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(10, 3); //alt option if 1 cost is too strong: 2 cost, 12 (+6)
        WithVar("TuneUpPower", 1); //no tooltip needed
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await PowerCmd.Apply<TuneUpPower>(choiceContext, Owner.Creature, DynamicVars["TuneUpPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}