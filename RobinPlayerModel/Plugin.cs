using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using RobinPlayerModel.Audio;
using RobinPlayerModel.Compatibility;
using RobinPlayerModel.Localization;
using UnityEngine;

namespace RobinPlayerModel;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.56")]
[BepInDependency("com.snmodding.BZtoSNLoader")]
[BepInDependency("Indigocoder.SuitLib", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    internal static AssetBundle SoundsBundle { get; private set; }

    internal static PlayerReplacementPreset ReplacementPreset { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;

        SoundsBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "robin_player_sn1");

        ReplacementPreset = EssentialRobinModelData.GetRobinModelPreset();
        
        WaitScreenHandler.RegisterEarlyLoadTask(PluginInfo.PLUGIN_NAME, EarlyLoadScreenTask);
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private static void EarlyLoadScreenTask(WaitScreenHandler.WaitScreenTask task)
    {
        ReplaceRyleyName.ReplaceNameOfRyleyEverywhere();
        SuitLibCompat.OverrideSuitMaterialIndicesIfInstalled();
        SuitLibCompat.RegisterCacheHoldIfInstalled(() => ReplacementPreset.ChangesCompleted);
        FemaleSoundReplacements.ReplaceSounds();
        ThirdPersonCompat.InitializeCompatibility();
    }
}