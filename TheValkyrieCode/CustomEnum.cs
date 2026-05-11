using BaseLib.Extensions;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheValkyrie.TheValkyrieCode;

public class CustomEnum
{
    [CustomEnum] public static CardTag Smite;
    [CustomEnum] public static CardKeyword Bless;
    
    public static HoverTip GetStaticHoverTip(string locEntry)
    {
        const string locTable = "static_hover_tips";
        return new HoverTip(
            new LocString(locTable, locEntry + ".title"), 
            new LocString(locTable, locEntry + ".description")
        );
    } 
}