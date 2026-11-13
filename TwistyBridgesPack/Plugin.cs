using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BZtoSNLoader;
using Nautilus.Handlers;
using UnityEngine.U2D;

namespace TwistyBridgesPack;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.56")]
[BepInDependency("com.snmodding.BZtoSNLoader", "1.0.1")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private const string PrefabPathsFileName = "TwistyBridgesPropPaths.txt";

    private bool _registeredPrefabs;

    public const string RootFolderName = "Twisty Bridges Pack";

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        WaitScreenHandler.RegisterEarlyAsyncLoadTask(PluginInfo.PLUGIN_NAME, RegisterPrefabs);
    }

    private IEnumerator RegisterPrefabs(WaitScreenHandler.WaitScreenTask task)
    {
        if (_registeredPrefabs) yield break;
        _registeredPrefabs = true;

        var filePath = Path.Combine(Path.GetDirectoryName(Assembly.Location), PrefabPathsFileName);
        new RegisterFromFile(filePath,
                path =>
                {
                    string prefabName =
                        path.Substring(path.LastIndexOf('/') + 1);
                    string containingFolder = GetBestFolderName(path.Split('/')[^2]);
                    new BelowZeroPrefab(new BZAssetLoader.AssetKey(path), new BelowZeroPrefab.Metadata
                    {
                        FolderPath = $"{RootFolderName}/{containingFolder}",
                        Prefix = "TwistyPack_",
                        CellLevel = GetBestCellLevelForPrefab(prefabName),
                        ZUp = GetZUp(prefabName),
                        SurfaceType = VFXSurfaceTypes.coral
                    }).Register();
                })
            .RegisterAll();

        TaskResult<SpriteAtlas> atlasResult = new TaskResult<SpriteAtlas>();
        yield return BZAssetLoader.LoadAssetAsync(
            new BZAssetLoader.AssetKey("Assets/AddressableResources/Atlases/Items.spriteatlas"), atlasResult);
        var atlas = atlasResult.Get();

        RibbonPlant.Register(atlas);
    }

    private LargeWorldEntity.CellLevel GetBestCellLevelForPrefab(string prefab)
    {
        if (prefab.Contains("twistybridges_bridge_01")) return LargeWorldEntity.CellLevel.Far;
        if (prefab.Contains("mushroom") || prefab.Contains("barnacles_01_single_01") || prefab.Contains("brain_coral")
            || prefab.Contains("coral_lump") || prefab.Contains("coral_shelf")) return LargeWorldEntity.CellLevel.Near;
        return LargeWorldEntity.CellLevel.Medium;
    }

    private bool GetZUp(string prefab)
    {
        return prefab.Contains("mohawk");
    }

    private string GetBestFolderName(string originalFolderName)
    {
        if (FolderNameMappings.TryGetValue(originalFolderName, out var folderName))
        {
            return folderName;
        }

        return originalFolderName;
    }

    private static readonly Dictionary<string, string> FolderNameMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        { "deeptwistybridges_bridgevines_01", "Deep Bridge Vines" },
        { "deeptwistybridges_plant_bulb_01", "Deep Bulbs" },
        { "deeptwistybridges_plant_tentacles_01", "Deep Tentacles" },
        { "deeptwistybridges_roots_medium_01", "Deep Roots" },
        { "Shared", "Luna Plants" },
        { "twistybridges_barnacles_01", "Barnacles" },
        { "twistybridges_brain_coral_plate_01", "Brain Coral Plate" },
        { "twistybridges_bridges_01", "Bridges" },
        { "twistybridges_coral_long_01", "Pink Narrowleaf" },
        { "twistybridges_coral_lumps", "Coral Lumps" },
        { "twistybridges_coral_shelf_01", "Coral Shelf" },
        { "twistybridges_coral_white_detail_01", "White Coral 1" },
        { "twistybridges_coral_white_detail_02", "White Coral 2" },
        { "twistybridges_mohawk_01", "Mohawk" },
        { "twistybridges_mushroom_01", "Bullseye shroom" },
        { "twistybridges_plant_cliff_01", "Crescent Moon Coral" },
        { "twistybridges_plant_large_01", "Twisted Mandrake" },
        { "twistybridges_rocks", "Rocks" }
    };
}