using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class DigYourHeelsIn : TheValkyrieCard
{
    public DigYourHeelsIn() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ArmorPower>(1, 1);
        WithPower<OverexertionPower>(9);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<ArmorPower>(choiceContext, Owner.Creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}