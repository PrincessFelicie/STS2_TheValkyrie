using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class BurnBright : TheValkyrieCard
{
    public BurnBright() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<StrengthPower>(1);
        WithPower<DexterityPower>(1);
        WithPower<BurnBrightPower>(1);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, -DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, -DynamicVars["DexterityPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<BurnBrightPower>(choiceContext, Owner.Creature, DynamicVars["BurnBrightPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}