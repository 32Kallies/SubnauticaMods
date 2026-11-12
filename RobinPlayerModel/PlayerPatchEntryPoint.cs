using HarmonyLib;

namespace RobinPlayerModel;

[HarmonyPatch(typeof(Player))]
public static class PlayerPatchEntryPoint 
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Awake))]
    private static void PatchPlayerAwake(Player __instance)
    {
        __instance.StartCoroutine(Plugin.ReplacementPreset.ReplacePlayerModel(__instance));
    }
}