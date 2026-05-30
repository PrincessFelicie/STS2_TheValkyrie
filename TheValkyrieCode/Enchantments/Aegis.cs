using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Enchantments;

public class Aegis : CustomEnchantmentModel
{
    protected override string CustomIconPath => "TheValkyrie/images/enchantments/aegis.png";
    
    public override bool CanEnchant(CardModel c)
    {
        return base.CanEnchant(c);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ArmorPower>()];

    public override bool IsStackable => true;
    
    public override bool HasExtraCardText => true;
    
    public override bool ShowAmount => true;
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await PowerCmd.Apply<TemporaryArmorPower>(choiceContext, Card.Owner.Creature, this.Amount, Card.Owner.Creature, Card);
    }
}