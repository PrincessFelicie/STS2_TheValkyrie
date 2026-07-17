using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using TheValkyrie.TheValkyrieCode.Character;

namespace TheValkyrie.TheValkyrieCode.Patches;

[HarmonyPatch(typeof(NBestiary), "CreateFilters")]
public class NBestiaryPatch
{
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    public static void Postfix(NBestiary __instance)
    {
        __instance.AddFilter(ModelDb.Character<Character.TheValkyrie>());
    }
}