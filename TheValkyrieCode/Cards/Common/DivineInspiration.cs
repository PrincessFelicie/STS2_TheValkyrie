using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class DivineInspiration : TheValkyrieCard
{
    public DivineInspiration() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        //WithBlock(2,2);
        WithPower<VigorPower>(2,2);
        WithVar("Quantity", 2);
        WithTip(typeof(Smite));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
        if (CombatState == null) return;
        await Smite.CreateInHand(Owner, DynamicVars["Quantity"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}