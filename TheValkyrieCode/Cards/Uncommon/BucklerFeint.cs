using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class BucklerFeint : TheValkyrieCard
{
    public BucklerFeint() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6, 2);
        WithPower<TemporaryThornsPower>(4, 1);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await PowerCmd.Apply<TemporaryThornsPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}