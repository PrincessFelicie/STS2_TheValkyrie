using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards;
using TheValkyrie.TheValkyrieCode.ModConfiguration;
using TheValkyrie.TheValkyrieCode.Patches.ModCompatPatches.AncientsAwakened;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.ModCompat.AncientsAwakened;

public class UnchosenStrike: TheValkyrieAncientsAwakenedCard
{
public UnchosenStrike() : base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    WithDamage(6, 3);
    WithPower<BleedPower>(2);
    WithTags(CardTag.Strike);
}

//affects the design of the card, not the compendium tab it gets listed in. like Trash Heap cards, for a vanilla point of comparison.
public override CardPoolModel VisualCardPool =>
    AncientsAwakenedCardRegisterPatch.GetPerfectedPoolOrFallback(
        base.VisualCardPool
    );

protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
{
    await CommonActions.CardAttack(this, play).Execute(choiceContext);
    if (play.Target != null)
    {
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
    }
}

protected override void OnUpgrade()
{
}
}