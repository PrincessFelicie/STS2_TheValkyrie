using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Brimming : TheValkyrieCard
{
    public Brimming() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("BrimmingPower", 7);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        //todo rework to "the first time you reach 10 cards in hand each turn, gain 7 block"
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<BrimmingPower>(choiceContext, Owner.Creature, DynamicVars["BrimmingPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}