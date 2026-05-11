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
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class ALyricsSheet : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1),
        new DynamicVar("Bless", 1)
    ];
    
    public override bool HasUponPickupEffect => true;
    
    public override bool IsStackable => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CustomEnum.Bless)];

    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        //FromDeckForEnchantment requires an enchantmentModel, we don't know what we want to enchant with yet so we need a generic; because it's a generic we need to do all the common isEnchantable checks in the filter
        foreach (CardModel card in await CardSelectCmd.FromDeckGeneric(this.Owner, prefs, 
                     c => (c.Enchantment == null || c.Enchantment.ShowAmount) //enchantment (or lack thereof) is valid
                           && c.Type is not (CardType.Curse or CardType.Status or CardType.Quest) //AND cardType is valid
                     ))
        {
            await BlessCmd.EnchantOrUpgradeEnchant(card, DynamicVars["Bless"].IntValue);
            CardCmd.Preview(card);
        }
    }
}