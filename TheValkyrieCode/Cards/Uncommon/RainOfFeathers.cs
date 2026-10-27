using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class RainOfFeathers : TheValkyrieCard
{
    public RainOfFeathers() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithPower<BleedPower>(1, 1);
        WithDamage(1, 1);
        WithVar("Hits", 2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).WithHitCount(this.DynamicVars["Hits"].IntValue).Execute(choiceContext);
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (cardSource != this || !props.IsPoweredAttack())
            return;
        await PowerCmd.Apply<BleedPower>(choiceContext, target, DynamicVars["BleedPower"].BaseValue, Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
    }
}