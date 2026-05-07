using BepInEx;
using Extensions;
using HarmonyLib;
using UnityEngine;

namespace AllInMod
{


    [BepInPlugin("com.yourname.slotsallin", "All-In Mod", "1.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            var harmony = new Harmony("com.hugsterf.allinmod");
            harmony.PatchAll();
            Debug.Log("[SlotsAllIn] Mod loaded!");
        }
    }

    [HarmonyPatch(typeof(GameBase))]
    public static class GameBasePatch
    {
        [HarmonyPatch("get_MaxBet")]
        [HarmonyPrefix]
        public static bool OverrideMaxBet(GameBase __instance, ref long __result)
        {
            try
            {
                if (__instance.isGoldenChipApplied)
                {
                    return true;
                }

                var moneyManager = NetworkSingleton<MoneyManager>.Instance;
                if (moneyManager == null)
                {
                    return true;
                }

                long balance = moneyManager.balance;
                long minBet = __instance.MinBet;

                __result = balance < minBet ? minBet : balance;
                return false;
            }
            catch (System.Exception e)
            {
                Debug.LogError("[SlotsAllIn] Error: " + e.Message);
                return true;
            }
        }
    }
}