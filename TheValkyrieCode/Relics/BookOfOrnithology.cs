using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class BookOfOrnithology : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Smites", 3), 
        new DynamicVar("BonusDamage", 2)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get => HoverTipFactory.FromCardWithCardHoverTips<Smite>();
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber > 1)
            return;
        
        this.Flash();
        for (int i = 0; i < DynamicVars["Smites"].BaseValue; ++i)
        {
            CardModel? card = await Smite.CreateInHand(Owner, combatState);
            if (card == null) return; //solves a warning
            CardCmd.Upgrade(card);
        }
    }

    public override decimal ModifyDamageAdditive(
        Creature? target, 
        decimal amount, 
        ValueProp props, 
        Creature? dealer,
        CardModel? cardSource)
    {
        return Owner.Creature != dealer 
               || !props.IsPoweredAttack() 
               || cardSource == null 
               || !cardSource.Tags.Contains<CardTag>(CustomEnum.Smite) ? 
            0 : DynamicVars["BonusDamage"].BaseValue;
    }
}