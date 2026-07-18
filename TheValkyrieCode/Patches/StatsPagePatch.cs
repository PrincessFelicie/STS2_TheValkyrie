using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using MegaCrit.Sts2.Core.Saves;
using TheValkyrie.TheValkyrieCode.Character;

namespace TheValkyrie.TheValkyrieCode.Patches;

[HarmonyPatch(typeof(NGeneralStatsGrid), "LoadStats")]
public class NGeneralStatsGridPatch
{
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    public static void Postfix(NGeneralStatsGrid __instance)
    {
        __instance.CreateCharacterSection(SaveManager.Instance.Progress, ModelDb.Character<Character.TheValkyrie>().Id);
    }
}