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

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class SongOfDivinity : TheValkyrieCard
{
    public SongOfDivinity() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Quantity", 2, 1);
        WithVar("Replay", 1);
        WithKeyword(CardKeyword.Exhaust);
        
        WithTip(typeof(Smite));
        WithTips(c => HoverTipFactory.FromEnchantment<Refrain>(c.DynamicVars["Replay"].IntValue));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;
        await Smite.CreateInHandWithEnchantment<Refrain>(Owner, DynamicVars["Quantity"].IntValue,DynamicVars["Replay"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}