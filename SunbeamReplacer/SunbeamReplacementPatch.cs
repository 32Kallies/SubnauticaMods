using HarmonyLib;

namespace SunbeamReplacer;

[HarmonyPatch(typeof(VFXSunbeam))]
public static class SunbeamReplacementPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(VFXSunbeam.Start))]
    private static void ReplaceSunbeamModel(VFXSunbeam __instance)
    {
        Plugin.Logger.LogInfo("Replacing Sunbeam model");
        SunbeamModelReplacer.ReplaceSunbeamModel(__instance.shipPrefab);
    }
}