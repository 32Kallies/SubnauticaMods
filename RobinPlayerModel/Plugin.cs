using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BZtoSNLoader;
using HarmonyLib;

namespace RobinPlayerModel;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.56")]
[BepInDependency("com.snmodding.BZtoSNLoader")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    internal static PlayerReplacementPreset ReplacementPreset { get; private set; }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        ReplacementPreset = new PlayerReplacementPreset
        {
            PlayerAssetPath = new BZAssetLoader.AssetKey("Assets/Models/player_female/player_view_female.fbx"),
            Replacements = new[]
            {
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/diveSuit/diveSuit_body_geo",
                    BzViewModelPath = "female_geo/base/female_base_body_geo"
                }
            }
        };
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}