using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using TheValkyrie.TheValkyrieCode.MetricsUpload;
using TheValkyrie.TheValkyrieCode.ModConfiguration;

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
        Harmony harmony = new(ModId);
        harmony.PatchAll();

        ModManager.OnMetricsUpload += ValkyrieMetrics.OnMetricsUpload;
        ModConfigRegistry.Register(ModId, new ValkyrieModConfig());
    }
    
    public static string GetModVersion()
    {
        var mod = ModManager.GetLoadedMods().FirstOrDefault(m => m.manifest?.id == "TheValkyrie");

        return mod?.manifest?.version ?? "unknown";
    }
}