using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

//Fatal is really weak in multiplayer. I could remove its condition and lower the damage gain, if I don't mind losing the flavor and increasing the overlap with The Scythe... Pretty big downsides, but the alternative is a card that's just non-viable outside of act 1.
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
    
    public RustedDagger() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(CurrentDamage);
        WithVar("Increase", 3, 1);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(StaticHoverTip.Fatal);
        WithVar("Active", 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        
        if (DynamicVars["Active"].BaseValue == 0)
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