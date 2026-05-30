using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class HallowedAura : TheValkyrieCard
{
    public HallowedAura() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
    {
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithKeyword(CardKeyword.Exhaust);
        WithPower<ArmorPower>(2);
    }
    
    public override CardMultiplayerConstraint MultiplayerConstraint
    {
        get => CardMultiplayerConstraint.MultiplayerOnly;
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (Creature creature in CombatState.GetTeammatesOf(Owner.Creature).Where( c => c.IsAlive && c.IsPlayer))
            await PowerCmd.Apply<ArmorPower>(choiceContext, creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}