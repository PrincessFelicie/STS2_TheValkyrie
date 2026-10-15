using BaseLib;
using BaseLib.Config;

namespace TheValkyrie.TheValkyrieCode.ModConfiguration;

[ConfigHoverTipsByDefault]
public class ValkyrieModConfig : SimpleModConfig
{
    public override void SetupConfigUI(Godot.Control optionContainer)
    {
        BaseLibMain.Logger.Info("Setting up Customized ModConfig " + this.GetType().FullName);
        this.GenerateOptionsForAllProperties(optionContainer);
        this.AddRestoreDefaultsButton(optionContainer);
        SimpleModConfig.SetupFocusNeighbors(optionContainer);
    }
    
    public static bool MetricsDataShare { get; set; } = false;
    
    [ConfigHideInUI]
    [ConfigIgnoreRestoreDefaults]
    public static bool MetricsDataSharePopupSeen { get; set; } = false;
    
    public static bool MysticLighterNerf { get; set; } = true;
    
    public static bool FunnyContent { get; set; } = true;
    
    [ConfigSection("CrossModContent", CollapsedByDefault = true)]
    public static bool AncientsAwakenedCustomCards { get; set; } = true;
}