using System.Collections;
using System.Linq;
using BZtoSNLoader;
using Nautilus.Assets;
using Nautilus.Handlers;
using UnityEngine;

namespace TwistyBridgesPack;

public class BelowZeroPrefab
{
    private readonly BZAssetLoader.AssetKey _assetKey;
    private readonly PrefabInfo _info;
    private readonly Metadata _data;

    public BelowZeroPrefab(BZAssetLoader.AssetKey assetKey, Metadata data)
    {
        _assetKey = assetKey;
        _data = data;
        
        string prefabName = GetPrefabName();
        string classId = GetClassId(prefabName);
        _info = new PrefabInfo(classId, GetPrefabPath(prefabName), TechType.None);
    }

    public void Register()
    {
        var prefab = new CustomPrefab(_info);
        prefab.SetGameObject(GetGameObjectAsync);
        prefab.Register();

        WorldEntityDatabaseHandler.AddCustomInfo(_info.ClassID, TechType.None, Vector3.one, _data.ZUp, _data.CellLevel);
    }

    private IEnumerator GetGameObjectAsync(IOut<GameObject> result)
    {
        var task = new TaskResult<GameObject>();
        yield return BZAssetLoader.LoadAssetAsync(_assetKey, task);
        var prefab = UWE.Utils.InstantiateDeactivated(task.Get());
        var identifier = prefab.AddComponent<PrefabIdentifier>();
        identifier.ClassId = _info.ClassID;
        identifier.prefabKey = _info.PrefabFileName;
        var lwe = prefab.AddComponent<LargeWorldEntity>();
        lwe.cellLevel = _data.CellLevel;
        prefab.AddComponent<SkyApplier>().renderers = prefab.GetComponentsInChildren<Renderer>(true);
        if (_data.SurfaceType != VFXSurfaceTypes.none)
            prefab.AddComponent<VFXSurface>().surfaceType = _data.SurfaceType;
        
        result.Set(prefab);
    }

    private string GetPrefabName() => _assetKey.Path.Split('/').Last().Split('.').First();

    private string GetPrefabPath(string name) => _data.FolderPath + "/" + name;

    private string GetClassId(string name)
    {
        if (_data.Prefix == null) return name;
        return _data.Prefix + name;
    }

    public sealed class Metadata
    {
        public string FolderPath { get; init; } = "BZPrefabs";
        public string Prefix { get; init; }
        public LargeWorldEntity.CellLevel CellLevel { get; init; } = LargeWorldEntity.CellLevel.Medium;
        public bool ZUp { get; init; }
        public VFXSurfaceTypes SurfaceType { get; init; }
    }
}