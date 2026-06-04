using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class AngelForm : TheValkyrieCard
{
    public AngelForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<ArmorPower>(1, 2);
        WithVar("AngelFormPower", 1);
        WithTip(typeof(Smite));
        //rebalance option: remove on equip armor gain, move per turn gain to 2, upgrade gives retain
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<ArmorPower>(choiceContext, Owner.Creature, DynamicVars["ArmorPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<AngelFormPower>(choiceContext, Owner.Creature, DynamicVars["AngelFormPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}