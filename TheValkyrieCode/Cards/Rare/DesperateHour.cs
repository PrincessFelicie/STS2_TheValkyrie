using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class DesperateHour : TheValkyrieCard
{
    public DesperateHour() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithTip(typeof(Smite));
        WithVar("Bless", 1); WithTip(CustomEnum.Bless);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        List<CardModel> list1 = PileType.Hand.GetPile(this.Owner).Cards.ToList();
        int exhaustCount = list1.Count;
        foreach (CardModel card in list1)
            await CardCmd.Exhaust(choiceContext, card);
        
        for (int i = 0; i < exhaustCount ; ++i)
        {
            if (CombatState == null) return;
            await Smite.CreateInHandBlessed(Owner, 1,  DynamicVars["Bless"].IntValue, CombatState);
            await Cmd.Wait(0.1f);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}