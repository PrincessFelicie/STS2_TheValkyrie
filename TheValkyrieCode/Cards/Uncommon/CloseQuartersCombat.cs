using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class CloseQuartersCombat : TheValkyrieCard
{
    public CloseQuartersCombat() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("CloseQuartersCombatPower", 34, 16);
        WithTip(typeof(OverexertionPower));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CloseQuartersCombatPower? power = await PowerCmd.Apply<CloseQuartersCombatPower>(choiceContext, Owner.Creature, DynamicVars["CloseQuartersCombatPower"].IntValue, Owner.Creature, this);
        if (power == null) return;
        power.DynamicVars["OverexertMult"].BaseValue *= 2;
        power.InvokeSecondAmountChanged();
    }

    protected override void OnUpgrade()
    {
    }
}