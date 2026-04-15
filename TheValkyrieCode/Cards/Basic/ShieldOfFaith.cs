using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Basic;



public class ShieldOfFaith : TheValkyrieCard
{
    public ShieldOfFaith() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithBlock(3);
        WithVars(new PowerVar<ArmorPower>(1));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<TemporaryArmorPower>(this.Owner.Creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(2);
        DynamicVars["ArmorPower"].UpgradeValueBy(1);
    }
}