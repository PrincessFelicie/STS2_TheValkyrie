using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class DivineInspiration : TheValkyrieCard
{
    public DivineInspiration() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(2,3);
        WithVar("Quantity", 2);
        WithTip(typeof(Smite));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        if (CombatState == null) return; //solves a warning
        await Smite.CreateInHand(Owner, DynamicVars["Quantity"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}