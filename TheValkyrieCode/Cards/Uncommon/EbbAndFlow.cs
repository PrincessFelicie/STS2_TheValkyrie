using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class EbbAndFlow : TheValkyrieCard
{
    public EbbAndFlow() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(2);
        WithTip(typeof(OverexertionPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<EbbAndFlowPower>(choiceContext, Owner.Creature, DynamicVars["Cards"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}