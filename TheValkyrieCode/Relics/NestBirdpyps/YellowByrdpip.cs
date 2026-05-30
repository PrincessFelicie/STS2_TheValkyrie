using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;
using TheValkyrie.TheValkyrieCode.Extensions;
using TheValkyrie.TheValkyrieCode.Pets;

namespace TheValkyrie.TheValkyrieCode.Relics.NestBirdpyps;

//first one obtained
public class YellowByrdpip : TheValkyrieRelic
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
        get => HoverTipFactory.FromCardWithCardHoverTips<ByrdSwoop>();
    }

    public override async Task AfterObtained()
    {
        Skin = new Rng((uint) (Owner.NetId + Owner.RunState.Rng.Seed)).NextItem(SkinOptions) ?? SkinOptions[0];
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<ByrdSwoop>(Owner), PileType.Deck), 1f);
        if (!CombatManager.Instance.IsInProgress)
            return;
        await this.SummonPet();
    }

    public override async Task BeforeCombatStart() => await this.SummonPet();

    private async Task SummonPet()
    {
        Creature creature = await PlayerCmd.AddPet<YellowByrdpipPet>(this.Owner);
    }
}