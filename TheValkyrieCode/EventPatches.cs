using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using TheValkyrie.TheValkyrieCode.Character;

namespace TheValkyrie.TheValkyrieCode;

[HarmonyPatch]
public class ValkyrieEventPatches
{
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

    //special thanks wyrdAutumn for this code 
    [HarmonyPatch(typeof(ByrdonisNest), "Eat", MethodType.Async)]
    public class ValkyrieNestEatPatch
    {
        private const string NewKey = "BYRDONIS_NEST.pages.EAT.valkyrieDescription";

        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(ValkyrieNestEatPatch), nameof(ReplaceEventText))
                ]);
        }

        private static string ReplaceEventText(string orig, ByrdonisNest instance)
        {
            return instance.Owner?.Character is Character.TheValkyrie ? NewKey : orig;
        }
    }

    [HarmonyPatch(typeof(ByrdonisNest), "Take", MethodType.Async)]
    public class ValkyrieNestTakePatch
    {
        private const string NewKey = "BYRDONIS_NEST.pages.TAKE.valkyrieDescription";

        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(ValkyrieNestTakePatch), nameof(ReplaceEventText))
                ]);
        }

        private static string ReplaceEventText(string orig, ByrdonisNest instance)
        {
            return instance.Owner?.Character is Character.TheValkyrie ? NewKey : orig;
        }
    }

    [HarmonyPatch(typeof(EventModel), "SetInitialEventState")]
    public static class ValkyrieByrdonisNestInitialPatch
    {
        [HarmonyPostfix]
        public static void Postfix(EventModel __instance)
        {
            if (__instance is ByrdonisNest && __instance.Owner != null &&
                __instance.Owner.Character is Character.TheValkyrie)
            {
                __instance.Description = new LocString("events", "BYRDONIS_NEST.pages.INITIAL.valkyrieDescription");
            }
        }
    }
}