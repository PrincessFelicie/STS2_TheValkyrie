using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Enchantments;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class FishingHook : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Shop;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(2),
        new DynamicVar("SanguineAmount", 2)
    ];
    
    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get => HoverTipFactory.FromEnchantment<Sanguine>(DynamicVars["SanguineAmount"].IntValue);
    }

    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        Sanguine canonicalEnchantment = ModelDb.Enchantment<Sanguine>();
        foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, canonicalEnchantment, DynamicVars["SanguineAmount"].IntValue, c => c?.Type == CardType.Attack, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, DynamicVars["SanguineAmount"].BaseValue);
            CardCmd.Preview(card);
        }
    }
}