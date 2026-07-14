using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class ShiningStar : TheValkyrieCard
{
    public ShiningStar() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("ShiningStarPower", 6, 2);
        WithTip(typeof(VigorPower));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<ShiningStarPower>(choiceContext, Owner.Creature, DynamicVars["ShiningStarPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}