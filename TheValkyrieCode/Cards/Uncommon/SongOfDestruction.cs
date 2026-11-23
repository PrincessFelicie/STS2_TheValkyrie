using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
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
        WithVar("Flay", 1, 1);
        
        WithTips(c => c.IsInCombat ? 
            [CustomHoverTipFactory.FromCombatSmiteWithEnchant(c.Owner, ModelDb.Enchantment<Flay>(), c.DynamicVars["Flay"].IntValue)] : 
            [CustomHoverTipFactory.FromCardWithEnchant(ModelDb.Card<Smite>(), false, ModelDb.Enchantment<Flay>(), c.DynamicVars["Flay"].IntValue)]
        );
        WithTips(c => HoverTipFactory.FromEnchantment<Flay>(c.DynamicVars["Flay"].IntValue));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target == null) return; //solves a warning
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<BleedPower>(choiceContext, play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        if (CombatState == null) return; //solves a warning
        await Smite.CreateInHandWithEnchantment<Flay>(Owner, DynamicVars["Quantity"].IntValue,DynamicVars["Flay"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}