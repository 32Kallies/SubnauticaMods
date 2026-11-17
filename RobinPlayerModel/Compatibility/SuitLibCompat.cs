using System;
using BepInEx.Bootstrap;
using SuitLib.API;

namespace RobinPlayerModel;

public static class SuitLibCompat
{
    private static bool IsSuitLibInstalled()
    {
        return Chainloader.PluginInfos.ContainsKey("Indigocoder.SuitLib");
    }

    public static void RefreshSuitLibForNewPlayerModelIfInstalled()
    {
        if (!IsSuitLibInstalled()) return;
        SuitLib.API.TextureCacheManager.RequestTextureCacheReset();
    }

    public static void RegisterCacheHoldIfInstalled(Func<bool> completionCondition)
    {
        if (!IsSuitLibInstalled()) return;
        SuitLib.API.CacheHoldManager.RegisterCacheHold(completionCondition);
    }

    public static void OverrideSuitMaterialIndicesIfInstalled()
    {
        if (!IsSuitLibInstalled()) return;
        MaterialIndexManager.SetBodyTextureIndexRemap(ModdedSuitsManager.VanillaModel.Dive, [1, 2]);
        MaterialIndexManager.SetArmTextureIndexRemap(ModdedSuitsManager.VanillaModel.Dive, [0, 3]);
        MaterialIndexManager.SetGloveTextureIndexRemap(ModdedSuitsManager.VanillaModel.Dive, [0, 1]);
    }
}