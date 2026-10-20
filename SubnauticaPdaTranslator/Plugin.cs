using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Nautilus.Handlers;

namespace SubnauticaPdaTranslator;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        ConsoleCommandsHandler.RegisterConsoleCommands(typeof(Commands));
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}