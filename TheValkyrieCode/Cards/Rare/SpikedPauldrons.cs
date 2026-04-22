using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class SpikedPauldrons : TheValkyrieCard
{
    public SpikedPauldrons() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<ArmorPower>(2);
        WithTip(typeof(ThornsPower));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (!this.Owner.Creature.HasPower<SpikedPauldronsPower>())
        {
            await PowerCmd.Apply<ThornsPower>(Owner.Creature, Owner.Creature.GetPowerAmount<ArmorPower>(), Owner.Creature, this);
        }
        await PowerCmd.Apply<SpikedPauldronsPower>(Owner.Creature, 1, Owner.Creature, this);
        if (this.IsUpgraded)
            await PowerCmd.Apply<ArmorPower>(Owner.Creature, DynamicVars["ArmorPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}