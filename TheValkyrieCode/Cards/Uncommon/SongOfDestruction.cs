using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;
using TheValkyrie.TheValkyrieCode.Utilities;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class SongOfDestruction : TheValkyrieCard
{
    public SongOfDestruction() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<BleedPower>(3, 1);
        WithPower<OverexertionPower>(4);
        WithVar("Quantity", 1);
        WithVar("Sanguine", 1, 1);
        
        WithTips(c => c.IsInCombat ? 
            [CustomHoverTipFactory.FromCombatSmiteWithEnchant(c.Owner, ModelDb.Enchantment<Sanguine>(), c.DynamicVars["Sanguine"].IntValue)] : 
            [CustomHoverTipFactory.FromCardWithEnchant(ModelDb.Card<Smite>(), false, ModelDb.Enchantment<Sanguine>(), c.DynamicVars["Sanguine"].IntValue)]
        );
        WithTips(c => HoverTipFactory.FromEnchantment<Sanguine>(c.DynamicVars["Sanguine"].IntValue));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null) return; //solves a warning
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        if (CombatState == null) return; //solves a warning
        await Smite.CreateInHandWithEnchantment<Sanguine>(Owner, DynamicVars["Quantity"].IntValue,DynamicVars["Sanguine"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}