using System.Collections;
using KallieʼsPropPack.MonoBehaviours.Corpses;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Corpses;

public class CreatureCorpse
{
    private readonly PrefabInfo _info;
    private readonly TechType _originalCreature;
    private readonly LargeWorldEntity.CellLevel _cellLevel;

    public bool UseDeadAnimation { get; init; } = true;
    
    public CreatureCorpse(string name, TechType originalCreature, LargeWorldEntity.CellLevel cellLevel)
    {
        var actualClassId = "Kallies_Corpse_" + name;
        _info = PrefabInfo.WithTechType(actualClassId).WithFileName("KallieʼsPropPack/Corpses/" + actualClassId);
        _originalCreature = originalCreature;
        _cellLevel = cellLevel;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(_info);
        prefab.SetGameObject(GetPrefab);
        prefab.Register();
    }

    private IEnumerator GetPrefab(IOut<GameObject> result)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(_originalCreature);
        yield return task;
        var leviathanPrefab = task.GetResult();
        var model = leviathanPrefab.GetComponentInChildren<Animator>().gameObject;
        var prefab = UWE.Utils.InstantiateDeactivated(model);
        foreach (var tm in prefab.GetComponentsInChildren<TrailManager>(true))
        {
            Object.DestroyImmediate(tm);
        }
        Object.DestroyImmediate(prefab.GetComponentInChildren<AnimateByVelocity>());
        if (UseDeadAnimation)
        {
            var animator = prefab.GetComponent<Animator>();
            animator.SetBool("dead", true);
            prefab.AddComponent<CreatureCorpseAnimation>().animator = animator;
        }
        var lodGroup = prefab.GetComponentInChildren<LODGroup>();
        if (lodGroup == null)
        {
            var renderers = prefab.GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                var nameLower = renderer.gameObject.name.ToLower();
                if (nameLower.Contains("lod") && !nameLower.Contains("lod0"))
                {
                    renderer.gameObject.SetActive(false);
                }
            }
        }
        
        foreach (var collider in prefab.GetComponentsInChildren<Collider>(true))
        {
            Object.DestroyImmediate(collider.gameObject.GetComponent<OnTouch>());
            Object.DestroyImmediate(collider);
        }

        foreach (var emitter in prefab.GetComponentsInChildren<FMOD_CustomEmitter>(true))
        {
            Object.DestroyImmediate(emitter);
        }
        
        PrefabUtils.AddBasicComponents(prefab, _info.ClassID, _info.TechType, _cellLevel);
        result.Set(prefab);
    }
}