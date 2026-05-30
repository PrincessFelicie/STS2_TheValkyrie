using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class WrittenInBlood : TheValkyrieCard
{
    private bool _active = true;
    
    [SavedProperty]
    public bool Active
    {
        get => this._active;
        set
        {
            this.AssertMutable();
            this._active = value;
        }
    }
    
    public override bool CanBeGeneratedInCombat => false;
    
    protected override bool ShouldGlowGoldInternal => this.Active;

    public WrittenInBlood() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithPower<BleedPower>(4, 2);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(CustomEnum.Bless);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        if (this.Active)
        {
            //apply the power first, otherwise the extra reward gets lost if the bleed kills the last creature of the fight
            await PowerCmd.Apply<WrittenInBloodPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }
        else
        {
            ThinkCmd.Play(new LocString("combat_messages", "ALREADY_TOOK_DAMAGE_THIS_FIGHT"), Owner.Creature, 2.0);
        }
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
    }
    
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber > 1)
            return;
        this.Active = true;
    }
    
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != this.Owner.Creature || result.UnblockedDamage <= 0)
        {
            return;
        }
        this.Active = false;
    }

    protected override void OnUpgrade()
    {
    }
}