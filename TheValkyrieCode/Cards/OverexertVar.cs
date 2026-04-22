using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheValkyrie.TheValkyrieCode.Cards;

public class OverexertVar : DynamicVar
{
    public const string Key = "Overexert";

    public OverexertVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip();
    }
}