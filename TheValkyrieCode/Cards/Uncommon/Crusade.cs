using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;
using TheValkyrie.TheValkyrieCode.Utilities;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Crusade : TheValkyrieCard
{
    public Crusade() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("CrusadeAttackPower", 2, 2); // WithVar instead of WithPower because we don't need a tooltip on the card, the card text says it all
        WithVar("CrusadeBlockPower", 1);
        WithTips(c => c.IsInCombat ? [CustomHoverTipFactory.FromCombatSmiteWithEnchant(c.Owner)] : [HoverTipFactory.FromCard<Smite>()]);
        WithTip(StaticHoverTip.Block);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CrusadePower? power = await PowerCmd.Apply<CrusadePower>(choiceContext, Owner.Creature, DynamicVars["CrusadeAttackPower"].IntValue, Owner.Creature, this);
        if (power == null) return;
        power.DynamicVars["BlockUpgrade"].BaseValue += DynamicVars["CrusadeBlockPower"].IntValue;
        power.InvokeSecondAmountChanged();
    }

    protected override void OnUpgrade()
    {
    }
}