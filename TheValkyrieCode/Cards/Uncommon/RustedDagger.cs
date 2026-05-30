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
    private const string _increaseKey = "Increase";
    private const int _baseDamage = 6;
    private int _currentDamage = 6;
    private int _increasedDamage;
    
    public override bool CanBeGeneratedInCombat => false;
    
    [SavedProperty]
    public int CurrentDamage
    {
        get => this._currentDamage;
        set
        {
            this.AssertMutable();
            this._currentDamage = value;
            this.DynamicVars.Damage.BaseValue = this._currentDamage;
        }
    }

    [SavedProperty]
    public int IncreasedDamage
    {
        get => this._increasedDamage;
        set
        {
            this.AssertMutable();
            this._increasedDamage = value;
        }
    }
    
    public RustedDagger() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(this.CurrentDamage);
        WithVar("Increase", 3, 2);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(StaticHoverTip.Fatal);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        
        bool shouldTriggerFatal = play.Target != null && play.Target.Powers.All( p => p.ShouldOwnerDeathTriggerFatal());
        
        AttackCommand attackCommand = await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        
        if (!shouldTriggerFatal || !attackCommand.Results.SelectMany(r => r).Any(r => r.WasTargetKilled))
            return;
        
        int intValue = DynamicVars["Increase"].IntValue;
        this.BuffFromPlay(intValue);
        if (this.DeckVersion is not RustedDagger deckVersion)
            return;
        deckVersion.BuffFromPlay(intValue);
        CardCmd.Preview(deckVersion);
    }
    
    private void BuffFromPlay(int extraDamage)
    {
        this.IncreasedDamage += extraDamage;
        this.UpdateDamage();
    }
    
    private void UpdateDamage() => this.CurrentDamage = 6 + this.IncreasedDamage;

    protected override void OnUpgrade() {}
    
    protected override void AfterDowngraded() => this.UpdateDamage();
}