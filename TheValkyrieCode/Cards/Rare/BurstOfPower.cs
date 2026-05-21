using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class BurstOfPower : TheValkyrieCard
{
    public BurstOfPower() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Bless", 1, 1); WithTip(CustomEnum.Bless);
        WithPower<OverexertionPower>(10);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
        foreach (CardModel card in PileType.Hand.GetPile(this.Owner).Cards)
        {
            await BlessCmd.EnchantOrUpgradeEnchant(card, DynamicVars["Bless"].IntValue);
        }
    }

    protected override void OnUpgrade()
    {
    }
}