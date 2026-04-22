using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class SongOfDestruction : TheValkyrieCard
{
    public SongOfDestruction() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<BleedPower>(4, 2);
        WithPower<OverexertionPower>(2);
        WithVar("Quantity", 1);
        WithVar("Sanguine", 2);
        WithTip(typeof(Smite));
        WithTip(typeof(Sanguine));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<BleedPower>(play.Target, DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<OverexertionPower>(Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        await Smite.CreateInHandWithEnchantment<Sanguine>(Owner, DynamicVars["Quantity"].IntValue,DynamicVars["Sanguine"].IntValue, CombatState);
    }

    protected override void OnUpgrade()
    {
    }
}