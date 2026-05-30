using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Extensions;
using TheValkyrie.TheValkyrieCode.Pets;

namespace TheValkyrie.TheValkyrieCode.Relics.NestBirdpyps;

//third one obtained
public class BlueByrdpip : TheValkyrieRelic
{
    protected override string PackedIconOutlinePath => "byrdpip_outline.png".RelicImagePath();
    
    private string _skin = SkinOptions[0];

    public override bool AddsPet => true;

    public override RelicRarity Rarity => RelicRarity.Event;

    public override bool HasUponPickupEffect => true;

    public override bool SpawnsPets => true;

    public static string[] SkinOptions =>
    [
        "version1",
        "version2",
        "version3",
        "version4"
    ];

    [SavedProperty]
    public string Skin
    {
        get => this._skin;
        set
        {
            this.AssertMutable();
            this._skin = value;
        }
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get => HoverTipFactory.FromCardWithCardHoverTips<TerritorialInstincts>();
    }

    public override async Task AfterObtained()
    {
        Skin = new Rng((uint) (Owner.NetId + Owner.RunState.Rng.Seed)).NextItem(SkinOptions) ?? SkinOptions[0];
        if (!CombatManager.Instance.IsInProgress)
            return;
        await this.SummonPet();
    }

    public override async Task BeforeCombatStart() => await this.SummonPet();

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber > 1)
            return;
        this.Flash();
        CardModel card = combatState.CreateCard<TerritorialInstincts>(Owner);
        CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
    
    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != this.Owner.Creature || card is not TerritorialInstincts)
            return false;
        modifiedCost = originalCost - 1;
        return true;
    }
    
    private async Task SummonPet()
    {
        await PlayerCmd.AddPet<BlueByrdpipPet>(this.Owner);
    }
}