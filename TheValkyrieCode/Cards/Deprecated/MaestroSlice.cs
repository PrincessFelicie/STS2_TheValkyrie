namespace TheValkyrie.TheValkyrieCode.Cards.Deprecated;

//kind of a boring design, and it causes a bunch of implementation questions I do not like.
//todo add it to trashheap event maybe? Keep it commentated out so it's not added to the card list for now
/*public class MaestroSlice : TheValkyrieCard
{
    public MaestroSlice() : base(1, CardType.Skill, CardRarity.Event, TargetType.AnyEnemy)
    {
        WithPower<BleedPower>(4, 2);
        WithCalculatedBlock(0,( card, target) =>target?.GetPowerAmount<BleedPower>() ?? 0);
        //WithCalculatedVar("DisplayBlock",4,( card, target) => target?.GetPowerAmount<BleedPower>() ?? 0, 2); //commentated out; becomes misleading when bloodied tithe and deepest cuts get involved. Better to let the player do the addition themselves.
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null)
            return;
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
        await CommonActions.CardBlock(this, DynamicVars.CalculatedBlock, play); //doesn't trigger if the creature has died from the bleed
    }

    protected override void OnUpgrade()
    {
    }
}*/