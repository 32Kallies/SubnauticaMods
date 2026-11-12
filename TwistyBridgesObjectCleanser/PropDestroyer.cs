using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TwistyBridgesObjectCleanser;

public sealed class PropDestroyer
{
    private PrefabInfo Info { get; }
    private TechType[] TechTypesToDestroy { get; }
    private string[] ClassIdsToDestroy { get; }
    private float DestroyRadius { get; }
    private Color Color { get; }

    public PropDestroyer(PrefabInfo info, TechType[] techTypesToDestroy, string[] classIdsToDestroy, float destroyRadius, Color color)
    {
        Info = info;
        TechTypesToDestroy = techTypesToDestroy;
        ClassIdsToDestroy = classIdsToDestroy;
        DestroyRadius = destroyRadius;
        Color = color;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetGameObject);
        prefab.Register();
    }

    private GameObject GetGameObject()
    {
        var obj = new GameObject(Info.ClassID);
        obj.SetActive(false);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, GetCellLevel(DestroyRadius));
        var destroy = obj.AddComponent<DestroyPropsInRange>();
        destroy.techTypesToDestroy = TechTypesToDestroy;
        destroy.classIdsToDestroy = ClassIdsToDestroy;
        destroy.radius = DestroyRadius;
        
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.DestroyImmediate(sphere.GetComponent<SphereCollider>());
        sphere.transform.parent = obj.transform;
        sphere.transform.localPosition = Vector3.zero;
        sphere.transform.localScale = Vector3.one * (DestroyRadius * 2f);

        var renderer = sphere.GetComponent<Renderer>();
        renderer.enabled = false;
        var material = new Material(MaterialUtils.StasisFieldMaterial);
        material.color = Color;
        renderer.sharedMaterial = material;
        
        var visualizer = sphere.AddComponent<DeleterVisualizer>();
        visualizer.renderer = renderer;
        
        return obj;
    }
    

    private static LargeWorldEntity.CellLevel GetCellLevel(float destroyRadius)
    {
        return destroyRadius switch
        {
            < 5 => LargeWorldEntity.CellLevel.Near,
            < 20 => LargeWorldEntity.CellLevel.Medium,
            _ => LargeWorldEntity.CellLevel.Far
        };
    }
}