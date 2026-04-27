using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Enchantments;

public class Sanguine : CustomEnchantmentModel
{
    protected override string CustomIconPath => "TheValkyrie/images/enchantments/sanguine.png";
    
    public override bool CanEnchant(CardModel c)
    {
        return base.CanEnchant(c) && c.TargetType != TargetType.RandomEnemy;
    }
    
    public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;
    
    public override bool HasExtraCardText => true;
    
    public override bool ShowAmount => true;
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        if (Card.CombatState == null) return; //solves a warning
        switch (Card.TargetType)
        {
            case TargetType.AllEnemies:
                await PowerCmd.Apply<BleedPower>(choiceContext, Card.CombatState.HittableEnemies, this.Amount, Card.Owner.Creature, Card);
                break;
            case TargetType.AnyEnemy:
                if (cardPlay?.Target == null) return; //solves a warning
                await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target, this.Amount, Card.Owner.Creature, Card);
                break;
            case TargetType.RandomEnemy:
                //I don't think this is the right task to handle this but I don't feel like refactoring rn
                break;
        }
    }
}