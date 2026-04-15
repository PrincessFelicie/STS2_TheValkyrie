using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class BurnBright : TheValkyrieCard
{
    public BurnBright() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(new PowerVar<StrengthPower>(1));
        WithVars(new PowerVar<DexterityPower>(1));
        WithVars(new PowerVar<BurnBrightPower>(1));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<StrengthPower>(this.Owner.Creature, -DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(this.Owner.Creature, -DynamicVars["DexterityPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<BurnBrightPower>(this.Owner.Creature, DynamicVars["DexterityPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}