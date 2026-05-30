using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using TheValkyrie.TheValkyrieCode.Character;

namespace TheValkyrie.TheValkyrieCode;

[HarmonyPatch(typeof(ColorfulPhilosophers))]
public static class ColorfulPhilosophersPatch
{
    [HarmonyPatch("CardPoolColorOrder", MethodType.Getter)]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    public static void Postfix(ref IEnumerable<CardPoolModel> __result)
    {
        __result = __result.Append(ModelDb.CardPool<TheValkyrieCardPool>());
    }
}