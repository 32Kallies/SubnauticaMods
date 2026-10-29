using System;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using System.Collections;
using DeExtinction.Mono;
using ECCLibrary.Mono;
using Nautilus.Utility;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

internal class ClownPincherPrefab : CreatureAsset
{
    private readonly Func<GameObject> _prefabModel;

    public ClownPincherPrefab(PrefabInfo prefabInfo, Func<GameObject> prefabModel, string encyImageName, string encyPopupName) : base(prefabInfo)
    {
        _prefabModel = prefabModel;
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, CommonDatabankPaths.Scavengers, null, null, 2,
            Plugin.AssetBundle.LoadAsset<Texture2D>(encyImageName),
            Plugin.AssetBundle.LoadAsset<Sprite>(encyPopupName));
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(_prefabModel,
            BehaviourType.SmallFish, EcoTargetType.SmallFish, 30)
        {
            SwimRandomData = new SwimRandomData(0.1f, 2f, new Vector3(10, 5, 10), 1.5f),
            AvoidObstaclesData = new AvoidObstaclesData(0.7f, 2f, false, 5f, 6f),
            StayAtLeashData = new StayAtLeashData(0.2f, 2f, 15f),
            LocomotionData = new LocomotionData(10, 0.4f),
            Mass = 4,
            CellLevel = LargeWorldEntity.CellLevel.Near,
            BioReactorCharge = 300f,
            AnimateByVelocityData = new AnimateByVelocityData(4),
            SizeDistribution = new AnimationCurve(new []{new Keyframe(0, 0.25f), new Keyframe(1, 1)}),
            SwimInSchoolData = new SwimInSchoolData(0.6f, 3f, 2f, 1f)
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.01f, 0.8f, 0.9f, 1f, true, true, ClassID));
        CreatureTemplateUtils.SetPreyEssentials(template, 4f,
            new PickupableFishData(TechType.Peeper, "WorldModel", "ViewModel"), new EdibleData(16, -3, false));
        template.SetCreatureComponentType<ClownPincherCreature>();
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var voiceEmitter = prefab.AddComponent<FMOD_CustomEmitter>();
        voiceEmitter.followParent = true;
        var voice = prefab.AddComponent<CreatureVoice>();
        voice.emitter = voiceEmitter;
        voice.minInterval = 40;
        voice.maxInterval = 60;
        voice.closeIdleSound = AudioUtils.GetFmodAsset("ClownPincherIdle");
        
        var eatingEmitter = prefab.AddComponent<FMOD_CustomLoopingEmitter>();
        eatingEmitter.followParent = true;
        eatingEmitter.SetAsset(AudioUtils.GetFmodAsset("ClownPincherEating"));

        var clownPincherCreatureComponent = (ClownPincherCreature) components.Creature;
        
        var scavengeBehaviour = prefab.AddComponent<ClownPincherScavengeBehaviour>();
        scavengeBehaviour.clownPincher = clownPincherCreatureComponent;
        scavengeBehaviour.swimVelocity = 3f;
        scavengeBehaviour.evaluatePriority = 0.8f;
        
        var nibble = prefab.SearchChild("Mouth").AddComponent<ClownPincherNibble>();
        nibble.clownPincher = clownPincherCreatureComponent;
        nibble.eatingEmitter = eatingEmitter;
        nibble.swimBehaviour = components.SwimBehaviour;

        clownPincherCreatureComponent.nibble = nibble;
        clownPincherCreatureComponent.scavengeBehaviour = scavengeBehaviour;
        clownPincherCreatureComponent.animateByVelocity = components.AnimateByVelocity;
        
        // var worldModel = prefab.SearchChild("WorldModel");

        /*
        var trailManagerBuilder = new TrailManagerBuilder(components,
            worldModel.SearchChild("Spine3", ECCStringComparison.StartsWith).transform, 1);
        trailManagerBuilder.SetTrailArrayToChildrenWithKeywords("Spine");
        trailManagerBuilder.Apply();
        */
        
#if BELOWZERO
        var viewModelTransform = prefab.GetComponent<FPModel>().viewModel.transform;
        viewModelTransform.localPosition = new Vector3(0.09f, 0, -0.05f);
        viewModelTransform.localEulerAngles = new Vector3(0, 233.5f, 261.4f);
#endif
        
        yield break;
    }
}