using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class PressYourAdvantage : TheValkyrieCard
{
    public PressYourAdvantage() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<WeakPower>(2);
        WithCalculatedBlock(0, 3, (card, target) => target != null ? target.Powers.Count(ShouldCountPower) : 0,
            ValueProp.Move, 0, 1);
        WithVar("DisplayBlockPerDebuff", 3, 1);
        WithCalculatedVar("DisplayBlock",3,3, 
            (card, target) => target != null ? target.Powers.Count(ShouldCountPower) + AccountForArtifactAndWeak(target) : 0,
            1, 1); //adds the extra 3(4) of the weak. AccountForAAW removes 3(4) if the creature has artifact or already has Weak.
        //sadly this causes issues with Frail, Dexterity, etc... probably need to rethink how the displayed number is implemented
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null) return;
        await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars["WeakPower"].IntValue, Owner.Creature, this);
        await CommonActions.CardBlock(this, DynamicVars.CalculatedBlock, play);
    }

    protected override void OnUpgrade()
    {
    }
    
    private static bool ShouldCountPower(PowerModel power)
    {
        return power.TypeForCurrentAmount == PowerType.Debuff && power is not ITemporaryPower;
    }

    private static int AccountForArtifactAndWeak(Creature target)
    {
        return target.HasPower<ArtifactPower>() || target.HasPower<WeakPower>() ? -1 : 0;
    }
}