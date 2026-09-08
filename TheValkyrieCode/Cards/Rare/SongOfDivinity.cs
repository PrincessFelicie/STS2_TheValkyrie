using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Utilities;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class SongOfDivinity : TheValkyrieCard
{
    public SongOfDivinity() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Quantity", 2, 1);
        WithVar("Replay", 1);
        WithKeyword(CardKeyword.Exhaust);
        
        WithTips(c => c.IsInCombat ? 
            [CustomHoverTipFactory.FromCombatSmiteWithEnchant(c.Owner, ModelDb.Enchantment<Refrain>(), c.DynamicVars["Replay"].IntValue)] : 
            [CustomHoverTipFactory.FromCardWithEnchant(ModelDb.Card<Smite>(), false, ModelDb.Enchantment<Refrain>(), c.DynamicVars["Replay"].IntValue)]
        );
        WithTips(c => HoverTipFactory.FromEnchantment<Refrain>(c.DynamicVars["Replay"].IntValue));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await Smite.CreateInHandWithEnchantment<Refrain>(Owner, DynamicVars["Quantity"].IntValue,DynamicVars["Replay"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}