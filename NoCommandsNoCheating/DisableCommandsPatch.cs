using HarmonyLib;

namespace NoCommandsNoCheating;

[HarmonyPatch]
public static class DisableCommandsPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DevConsole), nameof(DevConsole.ToggleState))]
    private static bool DisableDevConsoleToggling()
    {
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DevConsole), nameof(DevConsole.SetState))]
    private static void ForceDevConsoleStateOff(ref bool value)
    {
        value = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TerrainDebugGUI), nameof(TerrainDebugGUI.OnGUI))]
    private static bool DisableTerrainGUI(TerrainDebugGUI __instance)
    {
        __instance.enabled = !__instance.enabled;
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GraphicsDebugGUI), nameof(GraphicsDebugGUI.OnGUI))]
    private static bool DisableGraphicsDebugGUI(GraphicsDebugGUI __instance)
    {
        __instance.enabled = !__instance.enabled;
        return false;
    }
}