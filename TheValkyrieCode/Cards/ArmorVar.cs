using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheValkyrie.TheValkyrieCode.Cards;

public class ArmorVar : DynamicVar
{
    public const string Key = "Armor";

    public ArmorVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip();
    }
}