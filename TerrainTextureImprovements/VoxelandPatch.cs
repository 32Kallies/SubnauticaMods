using HarmonyLib;

namespace TerrainTextureImprovements;

[HarmonyPatch(typeof(Voxeland))]
internal static class VoxelandPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Voxeland.LoadPalette))]
    private static void Patch(Voxeland __instance)
    {
        TerrainReplacements.ReplaceBlockTypes(__instance);
    }
}