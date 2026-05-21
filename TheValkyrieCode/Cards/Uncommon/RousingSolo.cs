using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class RousingSolo : TheValkyrieCard
{
    public RousingSolo() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
    {
        WithPower<OverexertionPower>(10);
        WithCards(1, 1);
        WithEnergy(1);
    }
    
    public override CardMultiplayerConstraint MultiplayerConstraint
    {
        get => CardMultiplayerConstraint.MultiplayerOnly;
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (Creature creature in CombatState.GetTeammatesOf(Owner.Creature).Where(c => c.IsAlive && c.IsPlayer && c != Owner.Creature))
        {
            if (creature.Player == null) return;
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, creature.Player);
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, creature.Player);
        }
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}