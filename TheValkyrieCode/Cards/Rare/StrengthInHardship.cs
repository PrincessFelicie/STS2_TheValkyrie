using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class StrengthInHardship : TheValkyrieCard
{
    public StrengthInHardship() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("StrengthInHardshipPower", 1); //no tooltip needed
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
        WithTip(typeof(OverexertionPower));
        WithTip(typeof(ArmorPower));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<StrengthInHardshipPower>(choiceContext, Owner.Creature, DynamicVars["StrengthInHardshipPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}