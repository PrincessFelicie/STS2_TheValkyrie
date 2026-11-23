using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Powers;

public sealed class TerritorialPurposePower : TheValkyriePower
{
    private class Data
    {
    }

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("PlayedUpgraded", 0),
        new DynamicVar("HasUpgradedThisTurn", 0),
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<ByrdSwoop>(),
        HoverTipFactory.FromCard<Peck>(),
        HoverTipFactory.FromCard<GodByrdBlurredPreview>(),
        HoverTipFactory.FromPower<ByrdStrengthPower>()
    ];
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        DynamicVars["HasUpgradedThisTurn"].BaseValue = 0;
        Flash();
        
        foreach (CardModel original in (await CardSelectCmd.FromHand(choiceContext, player, new CardSelectorPrefs(SelectionScreenPrompt, Amount), c => c is not (ByrdSwoop or Peck or GodByrd), this)).ToList())
        {
            List<CardModel> possibleCards = [ModelDb.Card<ByrdSwoop>(), ModelDb.Card<Peck>(), ModelDb.Card<GodByrd>()];
            CardModel? replacement = Owner.Player.PlayerRng.GetRng(PlayerRngType.Transformations)
                .WeightedNextItem(possibleCards, (c => c is GodByrd ? 0.2f : 1f));
            if (replacement is null) return;
            CardModel createdCard = CombatState.CreateCard(replacement, Owner.Player);
            if (DynamicVars["HasUpgradedThisTurn"].BaseValue < DynamicVars["PlayedUpgraded"].BaseValue)
            {
                CardCmd.Upgrade(createdCard);
                DynamicVars["HasUpgradedThisTurn"].BaseValue++;
            }
            if (original.Enchantment != null)
            {
                EnchantmentModel enchantment = original.Enchantment.CanonicalInstance;
                CardCmd.Enchant(enchantment.ToMutable(), createdCard, original.Enchantment.Amount);
            }
            await CardCmd.Transform(original, createdCard);
        }
        await PowerCmd.Apply<ByrdStrengthPower>(choiceContext, Owner, Amount, Owner, null);
    }
}