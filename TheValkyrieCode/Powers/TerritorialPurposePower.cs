using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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
        new BoolVar("IsUpgraded", false),
    ];
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        this.Flash();
        foreach (CardModel original in (await CardSelectCmd.FromHand(choiceContext, player, new CardSelectorPrefs(SelectionScreenPrompt, 1), c => c is not (ByrdSwoop or Peck), this)).ToList())
        {
            if (this.DynamicVars["TurnCounter"].BaseValue % 2 == 0)
            {
                await CardCmd.TransformTo<ByrdSwoop>(original);
            }
            else
            {
                await CardCmd.TransformTo<Peck>(original);
            }
            if (this.DynamicVars["IsUpgraded"].BaseValue == 1)
                CardCmd.Upgrade(original);
        }
        await PowerCmd.Apply<ByrdStrengthPower>(choiceContext, this.Owner, 1, Owner, null);
        this.DynamicVars["TurnCounter"].BaseValue++;
    }
}