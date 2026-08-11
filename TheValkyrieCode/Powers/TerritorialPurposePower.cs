using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

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
        new DynamicVar("TurnCounter", 0),
        new DynamicVar("PlayedUpgraded", 0),
        new DynamicVar("HasUpgradedThisTurn", 0),
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<ByrdSwoop>(),
        HoverTipFactory.FromCard<Peck>(),
        HoverTipFactory.FromPower<ByrdStrengthPower>()
    ];
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        this.DynamicVars["HasUpgradedThisTurn"].BaseValue = 0;
        this.Flash();
        
        foreach (CardModel original in (await CardSelectCmd.FromHand(choiceContext, player, new CardSelectorPrefs(SelectionScreenPrompt, this.Amount), c => c is not (ByrdSwoop or Peck), this)).ToList())
        {
            if (Owner.Player.PlayerRng.GetRng(PlayerRngType.Transformations).NextBool())
            {
                CardModel replacement = CombatState.CreateCard<ByrdSwoop>(Owner.Player);
                if (this.DynamicVars["HasUpgradedThisTurn"].BaseValue < this.DynamicVars["PlayedUpgraded"].BaseValue)
                {
                    CardCmd.Upgrade(replacement);
                    this.DynamicVars["HasUpgradedThisTurn"].BaseValue++;
                }
                await CardCmd.Transform(original, replacement);
            }
            else
            {
                CardModel replacement = CombatState.CreateCard<Peck>(Owner.Player);
                if (this.DynamicVars["HasUpgradedThisTurn"].BaseValue < this.DynamicVars["PlayedUpgraded"].BaseValue)
                {
                    CardCmd.Upgrade(replacement);
                    this.DynamicVars["HasUpgradedThisTurn"].BaseValue++;
                }
                await CardCmd.Transform(original, replacement);
            }
            this.DynamicVars["TurnCounter"].BaseValue++;
        }
        await PowerCmd.Apply<ByrdStrengthPower>(choiceContext, this.Owner, this.Amount, Owner, null);
    }
}