using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class IcarusComplex : TheValkyrieCard
{
    public IcarusComplex() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("IcarusComplexPower", 75, 25);
        WithTip(typeof(VigorPower));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<IcarusComplexPower>(choiceContext, Owner.Creature, DynamicVars["IcarusComplexPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}