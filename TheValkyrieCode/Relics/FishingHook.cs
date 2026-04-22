using BaseLib.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class FishingHook : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Shop;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(2),
        new DynamicVar("SanguineAmount", 2)
    ];
    
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
        foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, canonicalEnchantment, DynamicVars["SanguineAmount"].IntValue, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, DynamicVars["SanguineAmount"].BaseValue);
            CardCmd.Preview(card);
        }
    }
}