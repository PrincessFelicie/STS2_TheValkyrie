using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class LoseYourself : TheValkyrieCard
{
    protected override bool ShouldGlowGoldInternal => this.Owner.Creature.GetPowerAmount<OverexertionPower>() >= 10;

    public LoseYourself() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ByrdStrengthPower>(2);
        WithPower<OverexertionPower>(10);

        WithUpgradingCardTip<Peck>();
        WithTip(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (this.Owner.Creature.GetPowerAmount<OverexertionPower>() >= 10)
        {
            await PowerCmd.Apply<ByrdStrengthPower>(choiceContext, Owner.Creature, DynamicVars["ByrdStrengthPower"].IntValue, Owner.Creature, this);
            await CardCmd.Exhaust(choiceContext, this);
            if (CombatState == null) return;
            CardModel createdCard = CombatState.CreateCard<Peck>(Owner);
            if (this.IsUpgraded)
                CardCmd.Upgrade(createdCard);
            await CardPileCmd.AddGeneratedCardToCombat(createdCard, PileType.Hand, Owner);
        }
        else
        {
            await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
    }
}