using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class HeavyArmor : TheValkyrieCard
{
    public HeavyArmor() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ArmorPower>(7, 2);
        WithPower<OverexertionPower>(5);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<TemporaryArmorPower>(choiceContext, Owner.Creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}