using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Inquisition : TheValkyrieCard
{
    public Inquisition() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithTip(typeof(Smite));
        WithTip(CustomEnum.Bless); //idk how to make the tip show up only when the card is upgraded. I believe I already tried using if IsUpgraded and it doesn't work in the base function.
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<InquisitionUpgradePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        if (this.IsUpgraded)
            await PowerCmd.Apply<InquisitionEnchantPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}