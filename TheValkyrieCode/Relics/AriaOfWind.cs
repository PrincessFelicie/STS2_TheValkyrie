using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class AriaOfWind : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Bless", 1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CustomEnum.Bless)];
    

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber > 1)
            return;
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
            await BlessCmd.EnchantOrUpgradeEnchant(card, DynamicVars["Bless"].IntValue);
        this.Flash();
    }
}