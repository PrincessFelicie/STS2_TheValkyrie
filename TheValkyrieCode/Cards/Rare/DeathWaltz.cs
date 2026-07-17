using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class DeathWaltz : TheValkyrieCard
{
    public DeathWaltz() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithPower<OverexertionPower>(6);
        WithDamage(2, 1);
    }
    
    protected override bool ShouldGlowRedInternal => Owner.Creature.GetPowerAmount<OverexertionPower>()+DynamicVars["OverexertionPower"].IntValue >= Owner.Creature.CurrentHp;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        if (Owner.Creature.GetPowerAmount<OverexertionPower>() >= Owner.Creature.CurrentHp)
        {
            if (LocalContext.IsMe(Owner))
                VfxCmd.PlayFullScreenInCombat("vfx/vfx_adrenaline", Owner.Creature);
            await Cmd.Wait(0.9f); //It seems there's a problem moving from the hit anim to the death anim, so we need a wait before we kill
            await CreatureCmd.Kill(Owner.Creature, true);
            await Cmd.CustomScaledWait(0.25f, 0.5f);
        }
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        CardLocation locationForCardPlay = base.GetResultLocationForCardPlay();
        if (locationForCardPlay.pileType == PileType.Discard)
            locationForCardPlay.pileType = PileType.Hand;
        return locationForCardPlay;
    }

    protected override void OnUpgrade()
    {
    }
}