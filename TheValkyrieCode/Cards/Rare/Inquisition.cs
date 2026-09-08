using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;
using TheValkyrie.TheValkyrieCode.Utilities;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Inquisition : TheValkyrieCard
{
    public Inquisition() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Bless", 0, 1);
        WithTips(c => [CustomHoverTipFactory.FromCardWithEnchant(ModelDb.Card<Smite>(), true, ModelDb.Enchantment<RandomBlessing>(), c.DynamicVars["Bless"].IntValue)]);
        WithTips(c => c.IsUpgraded ? [HoverTipFactory.FromKeyword(CustomEnum.Bless)]:[]);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        InquisitionPower? power = await PowerCmd.Apply<InquisitionPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        if (power == null) return;
        power.DynamicVars["Bless"].BaseValue += DynamicVars["Bless"].IntValue;
        power.InvokeSecondAmountChanged();
    }

    protected override void OnUpgrade()
    {
    }
}