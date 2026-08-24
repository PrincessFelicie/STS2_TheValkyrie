using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Saves.Runs;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class RustedDagger : TheValkyrieCard
{
    private const string _increaseKey = "Increase"; //from code for TheScythe... is this unused?
    private const int _baseDamage = 6;
    private int _currentDamage = 6;
    private int _increasedDamage;
    
    public override bool CanBeGeneratedInCombat => false;
    
    [SavedProperty]
    public int CurrentDamage
    {
        get => _currentDamage;
        set
        {
            AssertMutable();
            _currentDamage = value;
            DynamicVars.Damage.BaseValue = _currentDamage;
        }
    }

    [SavedProperty]
    public int IncreasedDamage
    {
        get => _increasedDamage;
        set
        {
            AssertMutable();
            _increasedDamage = value;
        }
    }
    
    protected override bool ShouldGlowGoldInternal => 
        DynamicVars["Active"].BaseValue == 1 &&
        CombatState.HittableEnemies.Any(c => c.GetPowerAmount<BleedPower>() >= 5);

    public RustedDagger() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(CurrentDamage);
        WithVar("Increase", 3, 1);
        WithKeyword(CardKeyword.Exhaust);
        WithVar("Active", 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        bool conditionMet =  play.Target?.GetPowerAmount<BleedPower>() >= 5;
        
        await CommonActions.CardAttack(this, play).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/gremlin_merc/gremlin_merc_attack_buff").WithHitFx("vfx/vfx_dramatic_stab").Execute(choiceContext); // BaseLib is seemingly overwriting all my sfx with the placeholder one...
        
        if (DynamicVars["Active"].BaseValue == 0 || !conditionMet)
            return;
        
        int intValue = DynamicVars["Increase"].IntValue;
        this.BuffFromPlay(intValue);
        if (this.DeckVersion is not RustedDagger deckVersion)
            return;
        deckVersion.BuffFromPlay(intValue);
        CardCmd.Preview(deckVersion);
        
        DynamicVars["Active"].BaseValue = 0;
    }
    
    private void BuffFromPlay(int extraDamage)
    {
        IncreasedDamage += extraDamage;
        UpdateDamage();
    }
    
    private void UpdateDamage() => CurrentDamage = _baseDamage + IncreasedDamage;

    protected override void OnUpgrade() {}
    
    protected override void AfterDowngraded() => UpdateDamage();
}