using System.Collections;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Utility;
using System.Reflection;
using DeExtinction.Mono;
using Nautilus.Handlers;
using UnityEngine;

namespace DeExtinction;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.56")]
[BepInDependency("com.lee23.ecclibrary", "2.2.4")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static AssetBundle AssetBundle { get; private set; }
    
    // Just a random value. Please don't copy this unless you want a mod incompatibility
    public static EcoTargetType ClownPincherFoodEcoTargetType { get; } = (EcoTargetType)5493558;

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "deextinctionassets");

        // Register localization
        LanguageHandler.RegisterLocalizationFolder();
        
        // Initialize custom prefabs
        CreaturePrefabManager.RegisterCreatures();
        CreaturePrefabManager.RegisterFood();

        // Register custom sounds
        CreatureAudio.RegisterAudio();

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        UWE.CoroutineHost.StartCoroutine(AddEcoTargetTypesToClownPincherFood());
        #if SN1
        UWE.CoroutineHost.StartCoroutine(ModifyBloomPlankton());
        #endif

        CreatureSpawnsToggler.CollectCreatures();
        CreatureSpawnsToggler.CreateModOptions(Config);
        CreatureSpawns.RegisterLootDistribution();
        CreatureSpawns.RegisterCoordinatedSpawns();
        CreatureSpawns.ModifyBaseGameSpawns();
    }

    private IEnumerator AddEcoTargetTypesToClownPincherFood()
    {
        foreach (var techType in ClownPincherCreature.ClownPincherFoods)
        {
            var task = CraftData.GetPrefabForTechTypeAsync(techType);
            yield return task;
            var result = task.GetResult();
            if (result) result.AddComponent<EcoTarget>().type = ClownPincherFoodEcoTargetType;
        }
    }

    private IEnumerator ModifyBloomPlankton()
    {
        var task = CraftData.GetPrefabForTechTypeAsync(TechType.Bloom);
        yield return task;
        var result = task.GetResult();
        if (!result) yield break;
        var locomotion = result.GetComponent<Locomotion>();
        locomotion.forwardRotationSpeed = 0.001f;
        locomotion.upRotationSpeed = 0.001f;
        result.GetComponent<Rigidbody>().maxAngularVelocity = 1;
    }
}