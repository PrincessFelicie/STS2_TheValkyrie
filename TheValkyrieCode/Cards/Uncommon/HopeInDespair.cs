using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class HopeInDespair : TheValkyrieCard
{
    public HopeInDespair() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(10);
        WithVar("HopeInDespairPower", 1);
        WithVar("OverexertionThreshold", 20, -4);
        WithTip(typeof(OverexertionPower));
    }
    
    protected override bool ShouldGlowGoldInternal => IsActive;
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (IsActive)
        {
            await CommonActions.CardBlock(this, play);
            await PowerCmd.Apply<HopeInDespairPower>(choiceContext, Owner.Creature, DynamicVars["HopeInDespairPower"].IntValue, Owner.Creature, this);
        }
    }

    private bool IsActive => Owner.Creature.GetPowerAmount<OverexertionPower>() >= DynamicVars["OverexertionThreshold"].IntValue;

    protected override void OnUpgrade()
    {
    }
}