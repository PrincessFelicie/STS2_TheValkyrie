using BaseLib.Config;

namespace TheValkyrie.TheValkyrieCode.ModConfiguration;

[ConfigHoverTipsByDefault]
public class ValkyrieModConfig : SimpleModConfig
{
    public static bool MysticLighterNerf { get; set; } = true;
}