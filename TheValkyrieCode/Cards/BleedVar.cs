using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheValkyrie.TheValkyrieCode.Cards;

public class BleedVar : DynamicVar
{
    public const string Key = "Bleed";

    public BleedVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip();
    }
}