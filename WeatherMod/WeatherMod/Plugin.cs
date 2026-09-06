using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using Nautilus.Utility.ModMessages;
using UnityEngine;
using WeatherMod.MessageReaders;

namespace WeatherMod;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
internal class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    
    public static AssetBundle AssetBundle { get; private set; }

    public static ModInbox Inbox { get; } = new ModInbox(PluginInfo.PLUGIN_GUID);

    internal static WeatherOptions Options { get; } = OptionsPanelHandler.RegisterModOptions<WeatherOptions>();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "weathermod");
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        
        ConsoleCommandsHandler.RegisterConsoleCommands(typeof(WeatherCommands));
        
        WeatherAudio.RegisterAll();

        Inbox.AddMessageReader(new SetWeatherReader());
        Inbox.AddMessageReader(new SetWeatherPausedReader());
        ModMessageSystem.RegisterInbox(Inbox);
    }
}