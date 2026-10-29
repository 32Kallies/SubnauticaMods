using System.Collections;
using System.Collections.Generic;
using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using ECCLibrary.Mono;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

public class TwisteelPrefab : CreatureAsset
{
    private readonly GameObject _prefabModel;
    private readonly bool _juvenile;

    public PrefabInfo EggInfo { get; set; }

    public TwisteelPrefab(PrefabInfo prefabInfo, GameObject prefabModel, bool juvenile) : base(prefabInfo)
    {
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, CommonDatabankPaths.Carnivores, null, null, _juvenile ? 4 : 8,
            Plugin.AssetBundle.LoadAsset<Texture2D>("Twisteel_Ency"),
            Plugin.AssetBundle.LoadAsset<Sprite>("Twisteel_Popup"));
        _prefabModel = prefabModel;
        _juvenile = juvenile;
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(_prefabModel,
            BehaviourType.Shark, EcoTargetType.Shark, 250)
        {
            CellLevel = LargeWorldEntity.CellLevel.Medium,
            SwimRandomData = new SwimRandomData(0.1f, _juvenile ? 4f : 6f, new Vector3(20, 4, 20), 5f, 0.8f),
            AggressiveToPilotingVehicleData = new AggressiveToPilotingVehicleData(25, 0.1f),
            AttackLastTargetData = new AttackLastTargetData(0.5f, _juvenile ? 13f : 15f, 0.6f, 9f),
            AnimateByVelocityData = new AnimateByVelocityData(_juvenile ? 11 : 13),
            AvoidObstaclesData = new AvoidObstaclesData(0.4f, _juvenile ? 4f : 6f, true, 9f, 10f),
            BioReactorCharge = 630,
            Mass = 300,
            StayAtLeashData = new StayAtLeashData(0.2f, _juvenile ? 4f : 6f, _juvenile ? 15 : 30),
            LocomotionData = new LocomotionData(10f, 0.3f),
            EyeFOV = -0.7f,
            BehaviourLODData = new BehaviourLODData(20, 100, 150),
            SizeDistribution = new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(0, 0.5f)),
            RespawnData = new RespawnData(false),
            TraitsData = new CreatureTraitsData(0.025f, 0.05f, 0.25f),
            AggressiveWhenSeeTargetList = new List<AggressiveWhenSeeTargetData>()
            {
                new(EcoTargetType.Shark, 1f, 30, 2),
                new(EcoTargetType.SmallFish, 0.5f, 15, 1, true, false, 0.3f),
                new(EcoTargetType.MediumFish, 0.5f, 15, 1, true, false, 0.5f),
            },
            SwimBehaviourData = new SwimBehaviourData(0.8f)
        };
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.01f, 0.1f, 0.6f, 1.5f, false, true,
            EggInfo.ClassID));
        
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var attackEmitter = prefab.AddComponent<FMOD_CustomEmitter>();
        attackEmitter.followParent = true;

        var voiceEmitter = prefab.AddComponent<FMOD_CustomEmitter>();
        voiceEmitter.followParent = true;

        var mouthObject = prefab.transform.Find("Twisteel/Twisteel_Anim/Spine1/Head").gameObject;
        var twisteelMeleeAttack = CreaturePrefabUtils.AddMeleeAttack<TwisteelMeleeAttack>(prefab, components, mouthObject, true, 30, 3, true);
        twisteelMeleeAttack.playerCameraAnimatedTransform = mouthObject.transform.Find("PlayerCam");
        twisteelMeleeAttack.attackEmitter = attackEmitter;
        
        var voice = prefab.AddComponent<CreatureVoice>();
        voice.emitter = voiceEmitter;
        voice.minInterval = 23;
        voice.maxInterval = 60;
        voice.closeIdleSound = AudioUtils.GetFmodAsset("TwisteelRoar");
        voice.farThreshold = 70;
        voice.playSoundOnStart = false;
        voice.animator = components.Animator;
        voice.animatorTriggerParam = "roar";

        var trailParent = prefab.SearchChild("Spine3");
        var trailManagerBuilder = new TrailManagerBuilder(components, trailParent.transform, 20);
        trailManagerBuilder.SetTrailArrayToChildrenWithKeywords("Spine");
        trailManagerBuilder.Apply();

        components.Rigidbody.inertiaTensor = new Vector3(1526.97f, 1526.97f, 62.35f);

        yield break;
    }
}