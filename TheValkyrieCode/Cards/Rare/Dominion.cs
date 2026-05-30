using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Dominion : TheValkyrieCard
{
    public Dominion() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("DominionPower", 2, 1);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<DominionPower>(choiceContext, Owner.Creature, DynamicVars["DominionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}