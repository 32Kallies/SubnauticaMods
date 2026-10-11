using BepInEx;
using BepInEx.Logging;
using Nautilus.Handlers;

namespace SubnauticaIsPain;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    // It might be a good idea to make this into an external JSON file so people can customize it
    private static readonly (string, string)[] Translations =
    {
        ("Play", "ENDURE THE SUFFERING"),
        ("NewGame", "CHOOSE YOUR PUNISHMENT"),
        ("StartNewGame", "RESTART THE CYCLE OF PAIN"),
        ("Survival", "Torment mode"),
        ("Freedom", "Soulless mode"),
        ("Hardcore", "Die in real life"),
        ("Creative", "Aimless agony"),
        ("Options", "MODIFY YOUR HELL"),
        ("Credits", "Blame your creators"),
        ("Quit", "Escape the horrors"),
        ("RunInBackground", "Suffer while multitasking"),
        ("NewsEnabled", "RECEIVE MESSAGES FROM CAPTORS"),
        ("Resolution", "Resolution of torture"),
        ("Detail", "Severity of pain"),
        ("Fullscreen", "Immersive fullscreen pain"),
        ("Gamma", "Blindingness"),
        ("MotionBlurQuality", "Sunbeam visual glitches"),
        ("BackToGame", "RESUME TORTURE"),
    };

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        RegisterLocalization();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void RegisterLocalization()
    {
        foreach (var entry in Translations)
        {
            LanguageHandler.SetLanguageLine(entry.Item1, entry.Item2);
        }
    }
}