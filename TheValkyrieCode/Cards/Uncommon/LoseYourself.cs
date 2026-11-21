using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class LoseYourself : TheValkyrieCard
{
    public LoseYourself() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ByrdStrengthPower>(2);
        WithPower<OverexertionPower>(5);

        WithUpgradingCardTip<Peck>();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<ByrdStrengthPower>(choiceContext, Owner.Creature, DynamicVars["ByrdStrengthPower"].IntValue, Owner.Creature, this);
        
        if (CombatState == null) return;
        CardModel createdCard = CombatState.CreateCard<Peck>(Owner);
        if (this.IsUpgraded)
            CardCmd.Upgrade(createdCard);
        if (this.Enchantment != null)
        {
            EnchantmentModel enchantment = this.Enchantment.CanonicalInstance;
            CardCmd.Enchant(enchantment.ToMutable(), createdCard, this.Enchantment.Amount);
        }
        if (!play.IsLastInSeries)
            return;
        this.TransformIntoAfterPlay = createdCard;
        
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        CardLocation locationForCardPlay = base.GetResultLocationForCardPlay();
        if (locationForCardPlay.pileType == PileType.Discard) //note to self: result location is determined *before* card effects trigger. not that it matters after the rework of this card
            locationForCardPlay.pileType = PileType.Hand;
        return locationForCardPlay;
    }

    protected override void OnUpgrade()
    {
    }
}