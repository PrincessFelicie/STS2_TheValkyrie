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
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Common;

public class SongOfProtection : TheValkyrieCard
{
    public SongOfProtection() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(5, 3);
        WithPower<OverexertionPower>(3);
        WithVar("Quantity", 1);
        WithVar("Nimble", 2);
        
        WithTip(typeof(Smite));
        WithTips(c => HoverTipFactory.FromEnchantment<Nimble>(c.DynamicVars["Nimble"].IntValue));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        if (CombatState == null) return;
        await Smite.CreateInHandWithEnchantment<Nimble>(Owner, DynamicVars["Quantity"].IntValue,DynamicVars["Nimble"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}