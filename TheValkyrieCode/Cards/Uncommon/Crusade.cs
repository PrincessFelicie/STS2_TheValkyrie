using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Crusade : TheValkyrieCard
{
    public Crusade() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("CrusadePower", 1, 1); // WithVar instead of WithPower because we don't need a tooltip on the card, the card text says it all
        WithTip(typeof(Smite));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<CrusadePower>(Owner.Creature, DynamicVars["CrusadePower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}