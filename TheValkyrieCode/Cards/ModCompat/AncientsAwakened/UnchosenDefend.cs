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

public class UnchosenDefend: TheValkyrieAncientsAwakenedCard
{
public UnchosenDefend() : base(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    WithBlock(5, 3);
    WithPower<ArmorPower>(2);
    WithTags(CardTag.Defend);
}

//affects the design of the card, not the compendium tab it gets listed in. like Trash Heap cards, for a vanilla point of comparison.
public override CardPoolModel VisualCardPool =>
    AncientsAwakenedCardRegisterPatch.GetPerfectedPoolOrFallback(
        base.VisualCardPool
    );

protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
{
    await CommonActions.CardBlock(this, play);
    await PowerCmd.Apply<TemporaryArmorPower>(choiceContext, this.Owner.Creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
}

protected override void OnUpgrade()
{
}
}