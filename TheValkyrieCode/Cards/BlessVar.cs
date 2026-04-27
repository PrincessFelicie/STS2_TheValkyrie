using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheValkyrie.TheValkyrieCode.Cards;

public class BlessVar : DynamicVar
{
    public const string Key = "Bless";

    public BlessVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip();
    }
}