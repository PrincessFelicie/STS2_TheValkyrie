using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using TheValkyrie.TheValkyrieCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using TheValkyrie.TheValkyrieCode.Cards.Basic;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Character;
public class TheValkyrie : PlaceholderCharacterModel
{
    public const string CharacterId = "TheValkyrie";

    public static readonly Color Color = new("8e400d");

    public override Color NameColor => Color;
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

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    public override string CustomArmPointingTexturePath
    {
        get => ImageHelper.GetImagePath($"ui/hands/the_valkyrie_arm_point.png");
    }

    public override string CustomArmRockTexturePath
    {
        get => ImageHelper.GetImagePath($"ui/hands/the_valkyrie_arm_rock.png");
    }

    public override string CustomArmPaperTexturePath
    {
        get => ImageHelper.GetImagePath($"ui/hands/the_valkyrie_arm_paper.png");
    }

    public override string CustomArmScissorsTexturePath
    {
        get => ImageHelper.GetImagePath($"ui/hands/the_valkyrie_arm_scissors.png");
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}