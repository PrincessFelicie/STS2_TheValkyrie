using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class DigYourHeelsIn : TheValkyrieCard
{
    public DigYourHeelsIn() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ArmorPower>(2, 1);
        WithPower<OverexertionPower>(9);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<ArmorPower>(Owner.Creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<OverexertionPower>(Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}