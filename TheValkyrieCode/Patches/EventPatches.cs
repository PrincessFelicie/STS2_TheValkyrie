using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using TheValkyrie.TheValkyrieCode.Character;

namespace TheValkyrie.TheValkyrieCode.Patches;

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
    [HarmonyPatch(typeof(EventModel), "SetInitialEventState")]
    public static class ValkyrieByrdonisNestInitialPatch
    {
        [HarmonyPostfix]
        public static void Postfix(EventModel __instance)
        {
            if (__instance is ByrdonisNest && __instance.Owner is { Character: Character.TheValkyrie })
            {
                __instance.Description = new LocString("events", "BYRDONIS_NEST.pages.INITIAL.valkyrieDescription");
            }
        }
    }

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
    public static class ValkyrieSelfHelpBookInitialPatch
    {
        [HarmonyPostfix]
        public static void Postfix(EventModel __instance)
        {
            if (__instance is SelfHelpBook && __instance.Owner is { Character: Character.TheValkyrie })
            {
                __instance.Description = new LocString("events", "SELF_HELP_BOOK.pages.INITIAL.valkyrieDescription");
            }
        }
    }
    
    //the No Options option doesn't work with this code because it's not an async method. can't be bothered to figure it out. that line is extra super duper uncommon anyway (how do you run into the self help book with literally 0 enchantable attacks and skills)
    /*[HarmonyPatch(typeof(SelfHelpBook), "SkipBook", MethodType.Async)]
    public class ValkyrieSelfHelpBookSkipBookPatch
    {
        private const string NewKey = "SELF_HELP_BOOK.pages.NO_OPTIONS.valkyrieDescription";

        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(ValkyrieSelfHelpBookSkipBookPatch), nameof(ReplaceEventText))
                ]);
        }

        private static string ReplaceEventText(string orig, SelfHelpBook instance)
        {
            return instance.Owner?.Character is Character.TheValkyrie ? NewKey : orig;
        }
    }*/

    //not satisfied with the custom dialogue for this event, actually. Will need to rewrite it before I'm willing to put it in the mod.
    /*[HarmonyPatch(typeof(SelfHelpBook), "ReadEntireBook", MethodType.Async)]
    public class ValkyrieSelfHelpBookReadEntireBookPatch
    {
        private const string NewKey = "SELF_HELP_BOOK.pages.READ_ENTIRE_BOOK.valkyrieDescription";

        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(ValkyrieSelfHelpBookReadEntireBookPatch), nameof(ReplaceEventText))
                ]);
        }

        private static string ReplaceEventText(string orig, SelfHelpBook instance)
        {
            return instance.Owner?.Character is Character.TheValkyrie ? NewKey : orig;
        }
    }
    
    [HarmonyPatch(typeof(SelfHelpBook), "ReadPassage", MethodType.Async)]
    public class ValkyrieSelfHelpBookReadPassagePatch
    {
        private const string NewKey = "SELF_HELP_BOOK.pages.READ_PASSAGE.valkyrieDescription";

        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(ValkyrieSelfHelpBookReadPassagePatch), nameof(ReplaceEventText))
                ]);
        }

        private static string ReplaceEventText(string orig, SelfHelpBook instance)
        {
            return instance.Owner?.Character is Character.TheValkyrie ? NewKey : orig;
        }
    }
    
    [HarmonyPatch(typeof(SelfHelpBook), "ReadTheBack", MethodType.Async)]
    public class ValkyrieSelfHelpBookReadTheBackPatch
    {
        private const string NewKey = "SELF_HELP_BOOK.pages.READ_THE_BACK.valkyrieDescription";

        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(ValkyrieSelfHelpBookReadTheBackPatch), nameof(ReplaceEventText))
                ]);
        }

        private static string ReplaceEventText(string orig, SelfHelpBook instance)
        {
            return instance.Owner?.Character is Character.TheValkyrie ? NewKey : orig;
        }
    }*/
}