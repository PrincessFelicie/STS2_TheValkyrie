using BaseLib.Extensions;
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

public class Contredanse: TheValkyrieAncientsAwakenedCard
{
public Contredanse() : base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    WithCalculatedVar("CardsAndEnergy", 1,( card, _) => 
        Math.Min(
            Math.Floor((decimal)card.Owner.Creature.GetPowerAmount<OverexertionPower>()/10)
            ,3)
        );
    WithCalculatedVar("CardsAndEnergyDisplay", 0,( card, _) => 
        Math.Min(
            Math.Floor((decimal)card.Owner.Creature.GetPowerAmount<OverexertionPower>()/10)
            ,3)
    );
    WithKeyword(CardKeyword.Retain);
    WithTip(typeof(OverexertionPower));
}

protected override bool ShouldGlowGoldInternal => this.Owner.Creature.GetPowerAmount<OverexertionPower>() >= 30;

protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
{
    await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
    await CardPileCmd.Draw(choiceContext, ((CalculatedVar) DynamicVars["CardsAndEnergy"]).Calculate(play.Target), this.Owner);
    await PlayerCmd.GainEnergy(((CalculatedVar) DynamicVars["CardsAndEnergy"]).Calculate(play.Target), this.Owner);
    await PowerCmd.Remove(Owner.Creature.GetPower<OverexertionPower>());
}

protected override void OnUpgrade()
{
    EnergyCost.UpgradeBy(-1);
}
}