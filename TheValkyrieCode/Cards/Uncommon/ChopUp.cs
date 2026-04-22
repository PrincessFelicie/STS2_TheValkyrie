using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class ChopUp : TheValkyrieCard
{
    public ChopUp() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<BleedPower>(3,1);
    }
    
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int energySpent = ResolveEnergyXValue();
        for (int i = 0; i < energySpent; ++i)
        {
            if (play.Target != null)
            {
                await PowerCmd.Apply<BleedPower>(play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}