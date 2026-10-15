using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards.ModCompat.AncientsAwakened;
using TheValkyrie.TheValkyrieCode.MetricsUpload;
using TheValkyrie.TheValkyrieCode.ModConfiguration;
using TheValkyrie.TheValkyrieCode.Patches.ModCompatPatches.AncientsAwakened;

namespace TheValkyrie.TheValkyrieCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "TheValkyrie"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        ModConfigRegistry.Register(ModId, new ValkyrieModConfig());
        
        Harmony harmony = new(ModId);
        harmony.PatchAll();

        ModManager.OnMetricsUpload += ValkyrieMetrics.OnMetricsUpload;
    }
    
    public static string GetModVersion()
    {
        var mod = ModManager.GetLoadedMods().FirstOrDefault(m => m.manifest?.id == ModId);

        return mod?.manifest?.version ?? "unknown";
    }
}