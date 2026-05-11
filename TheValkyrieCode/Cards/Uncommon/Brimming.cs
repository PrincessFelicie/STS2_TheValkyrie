using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Brimming : TheValkyrieCard
{
    public Brimming() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("BrimmingPower", 7);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<BrimmingPower>(choiceContext, Owner.Creature, DynamicVars["BrimmingPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}