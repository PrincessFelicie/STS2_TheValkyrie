using BaseLib;
using BaseLib.Config;
using Godot;

namespace TheValkyrie.TheValkyrieCode.ModConfiguration;

[ConfigHoverTipsByDefault]
public class ValkyrieModConfig : SimpleModConfig
{
    public override void SetupConfigUI(Control optionContainer)
    {
        BaseLibMain.Logger.Info("Setting up Customized ModConfig " + GetType().FullName);
        GenerateOptionsForAllProperties(optionContainer);
        AddRestoreDefaultsButton(optionContainer);
        SetupFocusNeighbors(optionContainer);
    }
    
    public static bool MetricsDataShare { get; set; } = false;
    
    [ConfigHideInUI]
    [ConfigIgnoreRestoreDefaults]
    public static bool MetricsDataSharePopupSeen { get; set; } = false;
    
    public static bool MysticLighterNerf { get; set; } = true;
    
    
    [ConfigSection("CustomEvents", CollapsedByDefault = true)]
    public static bool FunnyContent { get; set; } = true;
    public static bool EventRootedLibrary { get; set; } = true;
    
    
    [ConfigSection("CrossModContent", CollapsedByDefault = true)]
    public static bool AncientsAwakenedCustomCards { get; set; } = true;
}