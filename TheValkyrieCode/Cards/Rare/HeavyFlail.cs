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

//Funny but derivative. Can we find more original?
public class HeavyFlail : TheValkyrieCard
{
    public HeavyFlail() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVar("DisplayExtraDamage", 3, 2);
        WithCalculatedDamage(16, 3, (card, _) => card.Owner.Creature.GetPowerAmount<ArmorPower>(),ValueProp.Move, 0, 2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
    }
}