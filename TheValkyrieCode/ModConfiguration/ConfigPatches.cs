using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheValkyrie.TheValkyrieCode.ModConfiguration;

[HarmonyPatch(typeof(MysticLighter))]
public static class MysticLighterPatch
{
    [HarmonyPatch("CanonicalVars", MethodType.Getter)]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    public static void Postfix(ref IEnumerable<DynamicVar> __result)
    {
        int amount = ValkyrieModConfig.MysticLighterNerf ? 6 : __result.First().IntValue;
        __result = [new DamageVar(amount, ValueProp.Unpowered)];
    }
}