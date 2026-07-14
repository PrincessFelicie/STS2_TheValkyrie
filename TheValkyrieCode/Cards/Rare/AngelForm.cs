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
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithVar("AngelFormPower", 1);
        WithTip(typeof(Smite));
        WithTip(typeof(ArmorPower));
        WithTip(CardKeyword.Retain);
        WithTip(CardKeyword.Ethereal);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<AngelFormPower>(choiceContext, Owner.Creature, DynamicVars["AngelFormPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}