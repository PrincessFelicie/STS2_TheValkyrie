using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class DeepBreath : TheValkyrieCard
{
    public DeepBreath() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(7, 3);
        WithVar("OverexertionRemove",10, 5);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, -Math.Min(DynamicVars["OverexertionRemove"].IntValue, Owner.Creature.GetPowerAmount<OverexertionPower>()), Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}