using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class AngelForm : TheValkyrieCard
{
    public AngelForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<ArmorPower>(1, 2);
        WithVar("AngelFormPower", 1);
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