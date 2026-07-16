using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Zeal : TheValkyrieCard
{
    public Zeal() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1, 1);
        WithEnergy(1);
        WithKeyword(CardKeyword.Exhaust);
        WithVar("Bless", 1); WithTip(CustomEnum.Bless);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        IEnumerable<CardModel> cardsDrawn = await CommonActions.Draw(this, choiceContext);
        foreach (CardModel card in cardsDrawn)
        {
            if (!card.IsUpgraded)
                CardCmd.Upgrade(card);

            await BlessCmd.EnchantOrUpgradeEnchant(card, 1);
            
            CardCmd.Preview(card);
        }
    }

    protected override void OnUpgrade()
    {
    }
}