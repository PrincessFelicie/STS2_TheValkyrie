using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class CloseQuartersCombat : TheValkyrieCard
{
    public CloseQuartersCombat() : base(0, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("CloseQuartersCombatPower", 1);
        WithTip(typeof(OverexertionPower));
        WithTip(typeof(VulnerablePower));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CloseQuartersCombatPower? power = await PowerCmd.Apply<CloseQuartersCombatPower>(choiceContext, Owner.Creature, DynamicVars["CloseQuartersCombatPower"].IntValue, Owner.Creature, this);
        if (power == null) return;
        if (this.IsUpgraded)
            power.DynamicVars["IsUpgraded"].BaseValue = 1;
    }

    protected override void OnUpgrade()
    {
    }
}