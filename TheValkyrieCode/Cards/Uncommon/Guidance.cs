using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Guidance : TheValkyrieCard
{
    public Guidance() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
    {
        WithPower<VigorPower>(5,3);
    }
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null) return;
        await PowerCmd.Apply<VigorPower>(choiceContext, play.Target, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}