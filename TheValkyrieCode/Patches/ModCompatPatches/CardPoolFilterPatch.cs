using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheValkyrie.TheValkyrieCode.Cards.ModCompat;
using TheValkyrie.TheValkyrieCode.Character;
using TheValkyrie.TheValkyrieCode.ModConfiguration;

namespace TheValkyrie.TheValkyrieCode.Patches.ModCompatPatches;

[HarmonyPatch]
public class CardPoolFilterPatch
{
    [HarmonyPatch(typeof(CardPoolModel), "AllCards", MethodType.Getter)]
    public static class ValkyriePoolPostfix
    {
        [HarmonyPostfix]
        public static void ValkyrieCardPoolPostfix(ref IEnumerable<CardModel> __result, CardPoolModel __instance)
        {
            if (__instance is not TheValkyrieCardPool && __instance is not TokenCardPool && __instance is not EventCardPool)
                return;
            
            List<CardModel> list = __result.ToList();
            
            //ancients awakened mod. Very popular and active
            //if we notice that ancients awakened's Perfect Cards exist, then we can assume the mod is loaded
            var ancientsAwakenedType =
                AccessTools.TypeByName("AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix.PerfectedPool");
            //if the mod isn't loaded or the mod compat has been turned off, we remove the custom cards from the valkyrie's list
            if (ancientsAwakenedType == null || !ValkyrieModConfig.AncientsAwakenedCustomCards)
            {
                list.RemoveAll(c => c is TheValkyrieAncientsAwakenedCard);
            }
            
            //hero expansion mod. 
            /*
            var mysteriousFlashlightType =
                AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomMysteriousFlashlightExtension");
            if (mysteriousFlashlightType == null)
            {
                list.RemoveAll(c => c is TheSorceressModHeroExpansionCard);
            }
            */
        
            //"More Neow" mod... this one is dormant, compat not priority.
            /*
            var deckboxType = AccessTools.TypeByName("MoreNeow.MoreNeowCode.Relics.Complex.UnfamiliarDeckbox");
            if (deckboxType == null)
            {
                list.RemoveAll(c => c is TheSorceressModMoreNeowCard);
            }
            */

            __result = list;
        }
    }
}