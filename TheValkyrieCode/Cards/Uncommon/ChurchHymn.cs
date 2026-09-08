using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;
using TheValkyrie.TheValkyrieCode.Utilities;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class ChurchHymn : TheValkyrieCard
{
    public ChurchHymn() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithTip(typeof(OverexertionPower));
        WithTips(c => c.IsInCombat ? [CustomHoverTipFactory.FromCombatSmiteWithEnchant(c.Owner)] : [HoverTipFactory.FromCard<Smite>()]);
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