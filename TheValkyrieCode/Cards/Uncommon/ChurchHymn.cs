using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class ChurchHymn : TheValkyrieCard
{
    public ChurchHymn() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithTip(typeof(OverexertionPower));
        WithTip(typeof(Smite));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;
        if (this.IsUpgraded)
            await Smite.CreateInHand(Owner,1, CombatState);
        
        await PowerCmd.Apply<ChurchHymnPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}