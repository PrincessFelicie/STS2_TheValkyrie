using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class WeightOfResponsibility : TheValkyrieCard
{
    public WeightOfResponsibility() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVar("DisplayExtraDamage", 2, 1);
        WithCalculatedDamage(9, 2, (card, _) => card.Owner.Deck.Cards.Count(c => c.DynamicVars.ContainsKey("OverexertionPower")), ValueProp.Move,0, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}