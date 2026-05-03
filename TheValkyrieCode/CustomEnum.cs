using BaseLib.Extensions;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheValkyrie.TheValkyrieCode;

public class CustomEnum
{
    [CustomEnum] public static CardTag Smite;
    [CustomEnum] public static CardKeyword Bless;
    [CustomEnum] public static StaticHoverTip HatchFromNest;
}