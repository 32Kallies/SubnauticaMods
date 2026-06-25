using System.Collections;
using DeExtinction.MaterialModifiers;
using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

public class PyrambassisPrefab : CreatureAsset
{
    public PrefabInfo EggInfo { get; set; }

    public PyrambassisPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, CommonDatabankPaths.Carnivores, null, null, 6,
            Plugin.AssetBundle.LoadAsset<Texture2D>("Pyrambassis_Ency"),
            Plugin.AssetBundle.LoadAsset<Sprite>("Pyrambassis_Popup"));
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(() => Plugin.AssetBundle.LoadAsset<GameObject>("Pyrambassis_Prefab"), BehaviourType.MediumFish, EcoTargetType.MediumFish, 400f)
        {
            SwimRandomData = new SwimRandomData(0.2f, 4f, new Vector3(30, 5, 30)),
            StayAtLeashData = new StayAtLeashData(0.3f, 4f, 30),
            AvoidObstaclesData = new AvoidObstaclesData(0.6f, 4f, false, 7f, 8f),
            Mass = 160,
            CellLevel = LargeWorldEntity.CellLevel.Medium,
            BioReactorCharge = 600,
            SizeDistribution = new AnimationCurve(new Keyframe(0, 0.9f), new Keyframe(1, 1.0f)),
            LocomotionData = new LocomotionData(10, 0.3f, 2f, 0.8f),
            FleeOnDamageData = new FleeOnDamageData(0.8f, 8f),
            AnimateByVelocityData = new AnimateByVelocityData(10f),
            BehaviourLODData = new BehaviourLODData(30, 90, 500)
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.025f, 0.25f, 0.5f, 1f, false, true, EggInfo.ClassID));
        
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var trailManagerBuilder = new TrailManagerBuilder(components, prefab.transform.Find("Pyrambassis/Armature/Spine1"), 1f);
        trailManagerBuilder.SetTrailArrayToChildrenWithKeywords("Spine");
        trailManagerBuilder.Apply();

        AddAntennaTrailManager(prefab, components, "AntennaL1");
        
        AddAntennaTrailManager(prefab, components, "AntennaR1");

        var acid = prefab.AddComponent<SecreteAcidWhenApproached>();
        acid.evaluatePriority = 0.8f;
        acid.maxReactDistance = 7f;
        var acidPrefab = Object.Instantiate(components.LiveMixin.damageEffect, prefab.transform, true);
        acidPrefab.SetActive(false);
        acid.acidPrefab = acidPrefab;

        CreaturePrefabUtils.AddDamageModifier(prefab, DamageType.Acid, 0f);
        
        yield break;
    }

    private void AddAntennaTrailManager(GameObject prefab, CreatureComponents components, string antennaName)
    {
        var trailManagerBuilder = new TrailManagerBuilder(components, prefab.transform.SearchChild(antennaName), 2.5f, 0.1f);
        trailManagerBuilder.SetTrailArrayToAllChildren();
        trailManagerBuilder.Apply();
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 2, 3, 3, new FresnelModifier(0.8f), new ColorModifier(new Color(1, 1, 1, 2)), new IgnoreParticleRenderers());

        foreach (var renderer in prefab.GetComponentsInChildren<Renderer>())
        {
            var material = renderer.material;
            material.SetFloat("_MyCullVariable", 0);
        }
    }
}