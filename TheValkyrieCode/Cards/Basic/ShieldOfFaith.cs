using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Basic;



public class ShieldOfFaith : TheValkyrieCard
{
    public ShieldOfFaith() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithBlock(3, 2);
        WithPower<ArmorPower>(1, 1);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<TemporaryArmorPower>(choiceContext, this.Owner.Creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}