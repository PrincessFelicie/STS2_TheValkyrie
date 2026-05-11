using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Boon : TheValkyrieCard
{
    public Boon() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("BoonPower",1, 1); //no need for tooltip on that
        WithTip(CustomEnum.Bless);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<BoonPower>(choiceContext, Owner.Creature, DynamicVars["BoonPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}