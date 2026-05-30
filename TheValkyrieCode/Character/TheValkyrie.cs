using BaseLib.Abstracts;
using BaseLib.Patches.UI;
using BaseLib.Utils.NodeFactories;
using TheValkyrie.TheValkyrieCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheValkyrie.TheValkyrieCode.Cards.Basic;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Character;
public class TheValkyrie : PlaceholderCharacterModel
{
    public const string CharacterId = "TheValkyrie";

    public static readonly Color Color = new("923c00");
    public override Color EnergyLabelOutlineColor => Color;
    
    public override string CustomAttackSfx => "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_ram";
    public override string CustomCastSfx => "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_war_chant";
    public override string CustomDeathSfx => "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_die";
    

    public override Color NameColor => Color;
    public override Color MapDrawingColor => Color;
    
    public override Color DialogueColor => new Color("590700");
    public override VfxColor SpeechBubbleColor => VfxColor.Orange;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 68;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeValkyrie>(),
        ModelDb.Card<StrikeValkyrie>(),
        ModelDb.Card<StrikeValkyrie>(),
        ModelDb.Card<StrikeValkyrie>(),
        ModelDb.Card<DefendValkyrie>(),
        ModelDb.Card<DefendValkyrie>(),
        ModelDb.Card<DefendValkyrie>(),
        ModelDb.Card<DefendValkyrie>(),
        ModelDb.Card<MakeItBleed>(),
        ModelDb.Card<ShieldOfFaith>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BookOfFaith>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<TheValkyrieCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheValkyrieRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheValkyriePotionPool>();
    
    
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    public override string CustomVisualPath => "/combat/creature_visuals/the_valkyrie_temp.tscn".ScenesPath();
    //public override string CustomRestSiteAnimPath => "/character/the_valkyrie_rest_site.png".ImagePath(); todo
    //public override string CustomMerchantAnimPath => "/character/the_valkyrie_merchant.png".ImagePath(); todo
    
    public override string CustomArmPointingTexturePath => "/ui/hands/the_valkyrie_arm_point.png".ImagePath();
    public override string CustomArmRockTexturePath => "/ui/hands/the_valkyrie_arm_rock.png".ImagePath(); //todo
    public override string CustomArmPaperTexturePath => "/ui/hands/the_valkyrie_arm_paper.png".ImagePath(); //todo
    public override string CustomArmScissorsTexturePath => "/ui/hands/the_valkyrie_arm_scissors.png".ImagePath(); //todo 

    
    public override string CustomEnergyCounterPath => "/combat/energy_counters/valkyrie_energy_counter.tscn".ScenesPath();
    public override string CustomIconTexturePath => "character_icon_valkyrie.png".CharacterUiPath();
    public override string CustomIconOutlineTexturePath => "character_icon_valkyrie_outline.png".CharacterUiPath();
    
    //public override string CustomCharacterSelectBg => "char_select_valkyrie_background.tscn".CharacterUiPath(); todo
    public override string CustomCharacterSelectIconPath => "char_select_valkyrie.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_valkyrie_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_valkyrie.png".CharacterUiPath();
    
    public override RelicIconData? CustomYummyCookie => 
        new RelicIconData(
            "yummy_cookie_valkyrie.png".BigRelicImagePath(),
            "yummy_cookie_valkyrie.png".RelicImagePath(),
            "yummy_cookie_valkyrie_outline.png".BigRelicImagePath()
            );
}