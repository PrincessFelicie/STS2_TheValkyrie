using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class SongOfSafety : TheValkyrieCard
{
    public SongOfSafety() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<OverexertionPower>(3);
        WithPower<ArmorPower>(2, 2);
        WithVar("Quantity", 2);
        WithVar("Aegis", 1);
        
        WithTip(typeof(Smite));
        WithTips(c => HoverTipFactory.FromEnchantment<Aegis>(c.DynamicVars["Aegis"].IntValue));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<TemporaryArmorPower>(choiceContext, Owner.Creature, DynamicVars["ArmorPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        if (CombatState == null) return; //solves a warning
        await Smite.CreateInHandWithEnchantment<Aegis>(Owner, DynamicVars["Quantity"].IntValue,DynamicVars["Aegis"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}