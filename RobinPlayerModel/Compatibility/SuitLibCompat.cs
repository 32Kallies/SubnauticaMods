using System;
using BepInEx.Bootstrap;

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
}