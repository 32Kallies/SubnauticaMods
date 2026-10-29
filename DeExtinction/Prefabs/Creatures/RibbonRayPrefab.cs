using System.Collections;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

public class RibbonRayPrefab : CreatureAsset
{
    public RibbonRayPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, CommonDatabankPaths.SmallHerbivores, null, null, 2,
            Plugin.AssetBundle.LoadAsset<Texture2D>("RibbonRay_Ency"),
            Plugin.AssetBundle.LoadAsset<Sprite>("RibbonRay_Popup"));
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(() => Plugin.AssetBundle.LoadAsset<GameObject>("RibbonRay_Prefab"), BehaviourType.MediumFish, EcoTargetType.MediumFish, 40f)
        {
            SwimRandomData = new SwimRandomData(0.2f, 2, new Vector3(15, 3, 15), 3),
            AvoidObstaclesData = new AvoidObstaclesData(0.6f, 2, false, 5f, 6f),
            Mass = 4f,
            CellLevel = LargeWorldEntity.CellLevel.Near,
            BioReactorCharge = 400f,
            SizeDistribution = new AnimationCurve(new Keyframe(0, 0.8f), new Keyframe(1, 1)),
            LocomotionData = new LocomotionData(10, 0.4f, 3, 0.1f),
            AnimateByVelocityData = new AnimateByVelocityData(4f, animationMaxTilt: 20, dampTime: 0.2f),
            StayAtLeashData = new StayAtLeashData(0.4f, 2, 25f)
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.1f, 0.7f, 0.9f, 1f, true, true, PrefabInfo.ClassID));
        CreatureTemplateUtils.SetPreyEssentials(template, 4f,
            new PickupableFishData(TechType.Peeper, "WorldModel", "ViewModel"), new EdibleData(12, -6, false));
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        components.Creature.activity = DeExtinctionUtils.StandardActivityCurve;
        
        /*
        var trailManagerBuilder1 = new TrailManagerBuilder(components, prefab.transform.SearchChild("LTail2Spade"), 3, 0.5f);
        trailManagerBuilder1.SetTrailArrayToAllChildren();
        trailManagerBuilder1.Apply();
        
        var trailManagerBuilder2 = new TrailManagerBuilder(components, prefab.transform.SearchChild("RTail2Spade"), 3, 0.5f);
        trailManagerBuilder2.SetTrailArrayToAllChildren();
        trailManagerBuilder2.Apply();
        */
#if BELOWZERO
        var viewModelTransform = prefab.GetComponent<FPModel>().viewModel.transform;
        viewModelTransform.localPosition = new Vector3(-0.02f, 0.02f, 0);
        viewModelTransform.localEulerAngles = new Vector3(0, 0, 5);
        viewModelTransform.localScale = Vector3.one * 0.4f;
#endif

        yield break;
    }
}