using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class SongOfBattle : TheValkyrieCard
{
    public SongOfBattle() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVar("Quantity", 3);
        WithVar("Sharp", 3, 1);
        
        WithTip(typeof(Smite));
        WithTips(c => HoverTipFactory.FromEnchantment<Sharp>(c.DynamicVars["Sharp"].IntValue));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await Smite.CreateInHandWithEnchantment<Sharp>(Owner, DynamicVars["Quantity"].IntValue,DynamicVars["Sharp"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}