using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BZtoSNLoader;
using Nautilus.Handlers;

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
    
    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        WaitScreenHandler.RegisterEarlyLoadTask(PluginInfo.PLUGIN_NAME, RegisterPrefabs);
    }

    private void RegisterPrefabs(WaitScreenHandler.WaitScreenTask task)
    {
        if (_registeredPrefabs) return;
        _registeredPrefabs = true;
        
        var filePath = Path.Combine(Path.GetDirectoryName(Assembly.Location), PrefabPathsFileName);
        new RegisterFromFile(filePath,
            path =>
            {
                string prefabName =
                    path.Substring(path.LastIndexOf('/') + 1);
                string containingFolder = path.Split('/')[^2];
                new BelowZeroPrefab(new BZAssetLoader.AssetKey(path), new BelowZeroPrefab.Metadata
                {
                    FolderPath = $"Twisty Bridges Pack/{containingFolder}",
                    Prefix = "TwistyPack_",
                    CellLevel = GetBestCellLevelForPrefab(prefabName),
                    ZUp = GetZUp(prefabName),
                    SurfaceType = VFXSurfaceTypes.coral
                }).Register();
            })
            .RegisterAll();
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
}