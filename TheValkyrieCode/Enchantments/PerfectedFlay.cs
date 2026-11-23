using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Enchantments;

public class PerfectedFlay : CustomEnchantmentModel
{
    //protected override string CustomIconPath => "TheValkyrie/images/enchantments/sanguine.png";
    protected override string CustomIconPath => "TheValkyrie/images/powers/power.png"; //todo replace with icon
    
    public override bool CanEnchant(CardModel c)
    {
        return base.CanEnchant(c) && c.DynamicVars.ContainsKey("HitCount");
    }
    
    public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>()];
    
    public override bool HasExtraCardText => true;
    
    public override bool IsStackable => true;
    
    public override bool ShowAmount => true;
    
    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (cardSource != Card || !props.IsPoweredAttack())
            return;
        await PowerCmd.Apply<BleedPower>(choiceContext, target, Amount, Card.Owner.Creature, Card);
    }
}