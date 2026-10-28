using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;

namespace SunbeamReplacer;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    internal static AssetBundle Bundle { get; private set; }
    internal static SunbeamReplacementData ReplacementData { get; private set; }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        Bundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "new_sunbeam_model");
        var replacementPath = Path.Combine(Path.GetDirectoryName(Assembly.Location), "Assets", "ReplacementData.json");
        if (!File.Exists(replacementPath))
        {
            throw new FileNotFoundException("Cannot load Sunbeam Replacement. File not found at path " + replacementPath);
        }

        try
        {
            ReplacementData = JsonUtility.FromJson<SunbeamReplacementData>(File.ReadAllText(replacementPath));
        }
        catch
        {
            Logger.LogError($"Error while loading SunbeamReplacementData JSON data (at path '{replacementPath}')");
            throw;
        }
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}