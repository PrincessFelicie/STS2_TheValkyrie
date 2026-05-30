using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class Prelude : TheValkyrieCard
{
    public Prelude() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithKeyword(CardKeyword.Innate);
        WithDamage(3, 2);
        WithCards( 2, 1);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        foreach (CardModel card2 in PileType.Draw.GetPile(Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).TakeRandom<CardModel>(DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardSelection))
        {
            CardCmd.Upgrade(card2);
            CardCmd.Preview(card2);
        }
    }

    protected override void OnUpgrade()
    {
    }
}