using System;
using BepInEx.Bootstrap;
using SuitLib.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RobinPlayerModel;

public static class SuitLibCompat
{
    private static readonly Version RequiredSuitLibVersion = new("2.1.0");

    private static bool _suitLibPolled;
    private static bool _suitLibInstalled;
    
    private static bool IsSuitLibInstalled()
    {
        if (_suitLibPolled)
        {
            return _suitLibInstalled;
        }
        
        _suitLibPolled = true;
        
        if (!Chainloader.PluginInfos.TryGetValue("Indigocoder.SuitLib", out var suitLibInfo))
        {
            _suitLibInstalled = false;
            return false;
        }

        if (suitLibInfo.Metadata.Version < RequiredSuitLibVersion)
        {
            var warner = new GameObject("SuitLibMissingWarner");
            warner.AddComponent<Warner>();
            Object.DontDestroyOnLoad(warner);
            warner.AddComponent<SceneCleanerPreserve>();
            throw new SuitLibraryOutOfDateException(RequiredSuitLibVersion);
        }

        _suitLibInstalled = true;
        return true;
    }

    public static void RefreshSuitLibForNewPlayerModelIfInstalled()
    {
        if (!IsSuitLibInstalled()) return;
        TextureCacheManager.RequestTextureCacheReset();
    }

    public static void RegisterCacheHoldIfInstalled(Func<bool> completionCondition)
    {
        if (!IsSuitLibInstalled()) return;
        CacheHoldManager.RegisterCacheHold(completionCondition);
    }

    public static void OverrideSuitMaterialIndicesIfInstalled()
    {
        if (!IsSuitLibInstalled()) return;
        MaterialIndexManager.SetBodyTextureIndexRemap(ModdedSuitsManager.VanillaModel.Dive, [1, 2]);
        MaterialIndexManager.SetArmTextureIndexRemap(ModdedSuitsManager.VanillaModel.Dive, [0, 3]);
        MaterialIndexManager.SetGloveTextureIndexRemap(ModdedSuitsManager.VanillaModel.Dive, [0, 1]);
    }

    private class SuitLibraryOutOfDateException(Version version)
        : Exception($"Suit Lib must be updated to {version}+ for compatibility with the Robin player model");

    private class Warner : MonoBehaviour
    {
        private void Start()
        {
            Warn();
            InvokeRepeating(nameof(Warn), 6, 6);
        }

        private void Warn()
        {
            ErrorMessage.AddMessage("<color=#FF0000>Please update Suit Library. Your player model may break.</color>");
        }
    }
}