using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class TerritorialInstincts : TheValkyrieCard
{
    public TerritorialInstincts() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("TerritorialPurposePower", 1); // WithVar instead of WithPower because we don't need a tooltip on the card, the card text says it all
        WithTip(typeof(Peck));
        WithTip(typeof(ByrdSwoop));
        WithTip(typeof(ByrdStrengthPower));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        PowerModel? power = await PowerCmd.Apply<TerritorialPurposePower>(choiceContext, Owner.Creature, DynamicVars["TerritorialPurposePower"].IntValue, Owner.Creature, this);
        if (this.IsUpgraded && power != null)
            power.DynamicVars["IsUpgraded"].BaseValue = 1;
    }

    protected override void OnUpgrade()
    {
    }
}