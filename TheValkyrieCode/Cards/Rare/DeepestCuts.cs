using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class DeepestCuts : TheValkyrieCard
{
    public DeepestCuts() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("DeepestCutsPower", 1); // WithVar instead of WithPower because we don't need a tooltip on the card, the card text says it all
        WithTip(typeof(BleedPower)); //We do want the tooltip for the Bleed mechanic though
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<DeepestCutsPower>(choiceContext, Owner.Creature, DynamicVars["DeepestCutsPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}