using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.Powers;
public sealed class BrimmingPower : TheValkyriePower
{
    private class Data
    {
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BoolVar("IsActive", true)
    ];

    protected override object InitInternalData()
    {
        return new Data();
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side || this.DynamicVars["IsActive"].BaseValue == 1) //at the start of our turn, reset the power...
            return;
        this.DynamicVars["IsActive"].BaseValue = 1;
        this.Flash();
    }
    
    public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        // check for a fully retained hand (pyramid, stable serum)
        if (this.DynamicVars["IsActive"].BaseValue == 0) //If the power has already activated this turn, ignore.
            return;
        if (Owner.Player == null) return;
        IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(Owner.Player).Cards;
        if (cards.Count <= 9) return;
        this.Flash();
        await CreatureCmd.GainBlock(Owner, this.Amount, ValueProp.Unpowered, null);
        this.DynamicVars["IsActive"].BaseValue = 0;
    }
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        // check after drawing a card
        if (this.DynamicVars["IsActive"].BaseValue == 0) //If the power has already activated this turn, ignore.
            return;
        if (Owner.Player == null) return;
        IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(Owner.Player).Cards;
        if (cards.Count <= 9) return;
        this.Flash();
        await CreatureCmd.GainBlock(Owner, this.Amount, ValueProp.Unpowered, null);
        this.DynamicVars["IsActive"].BaseValue = 0;
    }
    
    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        // check after creating a card
        if (this.DynamicVars["IsActive"].BaseValue == 0) //If the power has already activated this turn, ignore.
            return;
        if (Owner.Player == null) return;
        IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(Owner.Player).Cards;
        if (cards.Count <= 9) return;
        this.Flash();
        await CreatureCmd.GainBlock(Owner, this.Amount, ValueProp.Unpowered, null);
        this.DynamicVars["IsActive"].BaseValue = 0;
    }
}