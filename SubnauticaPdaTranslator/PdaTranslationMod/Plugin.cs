using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using FMOD;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using Newtonsoft.Json;
using UnityEngine;

namespace PdaTranslationMod;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private static bool _registered;
    
    internal static Dictionary<string, LocalizedEntry> LocalizedEntries { get; private set; }
    internal static Dictionary<string, TranslatedDialogue> TranslatedDialogue { get; private set; } // key: localization key

    private const string FmodAssetFormat = "{0}_TRANSLATED";
    
    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        
        WaitScreenHandler.RegisterAsyncLoadTask(PluginInfo.PLUGIN_NAME, LoadMod);
        
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private IEnumerator LoadMod(WaitScreenHandler.WaitScreenTask task)
    {
        if (_registered)
            yield break;
        
        _registered = true;

        // Load PDA logs
        task.Status = "Loading PDA logs";
        var jsonFile = GetJsonFilePath();
        string jsonText = File.ReadAllText(jsonFile);
        LocalizedEntries = JsonConvert.DeserializeObject<Dictionary<string, LocalizedEntry>>(jsonText);

        TranslatedDialogue = new Dictionary<string, TranslatedDialogue>(StringComparer.InvariantCultureIgnoreCase);
        foreach (string file in Directory.GetFiles(GetLogsFolderPath()))
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                if (!LocalizedEntries.TryGetValue(fileName, out _)) // dispose the entry data as it's unneeded
                {
                    Logger.LogError($"Did not find log entry information for log file {fileName} in JSON");
                    continue;
                }
                var sound = AudioUtils.CreateSound(file, AudioUtils.StandardSoundModes_2D);
                var id = string.Format(FmodAssetFormat, fileName);
                CustomSoundHandler.RegisterCustomSound(id, sound, AudioUtils.BusPaths.PDAVoice);

                FMODAsset newAsset = AudioUtils.GetFmodAsset(id);
                TranslatedDialogue.Add(id, new TranslatedDialogue(id, newAsset));
            }
            catch (Exception e)
            {
                Logger.LogError($"Exception thrown while loading sound from file '{file}': " + e);
            }
        }
    }

    private string GetModFolderPath()
    {
        return Path.GetDirectoryName(Assembly.Location);
    }

    private string GetLogsFolderPath()
    {
        return Path.Combine(GetModFolderPath(), "Logs");
    }

    private string GetNotificationsFolderPath()
    {
        return Path.Combine(GetModFolderPath(), "Notifications");
    }

    private string GetJsonFilePath()
    {
        var jsonFiles = Directory.GetFiles(GetModFolderPath(), "*.json");
        if (jsonFiles.Length == 0)
        {
            throw new FileNotFoundException($"Could not find a json file with proper translations in the '{GetModFolderPath()}' folder.");
        }

        if (jsonFiles.Length > 1)
        {
            Logger.LogWarning($"Ambiguous match: found multiple language files in the {PluginInfo.PLUGIN_NAME} mod folder.");
        }

        return jsonFiles[0];
    }
}