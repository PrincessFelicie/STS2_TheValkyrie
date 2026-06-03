using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class BookOfFaith : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Smites", 3)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get => HoverTipFactory.FromCardWithCardHoverTips<Smite>();
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber > 1)
            return;
        this.Flash();
        await Smite.CreateInHand(Owner, DynamicVars["Smites"].IntValue, combatState);
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<BookOfOrnithology>();
    }
}