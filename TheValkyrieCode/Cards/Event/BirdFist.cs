using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Event;

//Whooops! Death Waltz!! Can't make that card!!! If we want to dust it off, we need to cap the number of Overexert events it can prevent per turn, but then that's extra card text.
/*public class BirdFist : TheValkyrieCard
{
    public BirdFist() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("BirdFistPower", 1);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> validPiles = PileType.Draw.GetPile(this.Owner).Cards.OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList().Concat(PileType.Discard.GetPile(this.Owner).Cards.OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList()).Concat(PileType.Hand.GetPile(this.Owner).Cards.OrderBy(c => c.Rarity).ThenBy((Func<CardModel, ModelId>) (c => c.Id)).ToList()).ToList();
        List<CardModel> validCards = validPiles.Where(c => c.Type is CardType.Skill).ToList();
        foreach (CardModel card in validCards)
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
        await PowerCmd.Apply<BirdFistPower>(choiceContext, Owner.Creature, DynamicVars["BirdFistPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}*/