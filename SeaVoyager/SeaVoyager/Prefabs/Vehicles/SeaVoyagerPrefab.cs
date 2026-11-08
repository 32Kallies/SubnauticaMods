using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using System.Collections;
using System.Collections.Generic;
using mset;
using SeaVoyager.Mono;
using UnityEngine;
using UWE;
using static CraftData;

namespace SeaVoyager.Prefabs.Vehicles;

public class SeaVoyagerPrefab
{
    private static readonly FMODAsset ClimbUpLongSound = Helpers.GetFmodAsset("event:/sub/cyclops/climb_front_up");
    private static readonly FMODAsset ClimbUpShortSound = Helpers.GetFmodAsset("event:/sub/cyclops/climb_back_up");

    private static readonly FMODAsset SlideDownSound =
        Helpers.GetFmodAsset("event:/sub/rocket/ladders/innerRocketShip_ladder_down");

    public PrefabInfo Info { get; } = PrefabInfo.WithTechType("SeaVoyager");

    public SeaVoyagerPrefab Register()
    {
        var prefab = new CustomPrefab(Info);

        Info.WithIcon(Plugin.assetBundle.LoadAsset<Sprite>("SeaVoyagerCraftIcon"));

        var craftingGadget = prefab.SetRecipe(new Nautilus.Crafting.RecipeData(
            new Ingredient(TechType.TitaniumIngot, 2),
            new Ingredient(TechType.Lubricant, 1),
            new Ingredient(TechType.Floater, 2),
            new Ingredient(TechType.AdvancedWiringKit, 1),
            new Ingredient(TechType.Glass, 2)
        ));

        craftingGadget
            .WithStepsToFabricatorTab("Vehicles")
            .WithCraftingTime(10f)
            .WithFabricatorType(CraftTree.Type.Constructor);

        prefab.SetPdaGroupCategory(TechGroup.Constructor, TechCategory.Constructor)
            .WithAnalysisTech(Plugin.assetBundle.LoadAsset<Sprite>("SeaVoyagerUnlockPopup"))
            .WithEncyclopediaEntry("Tech/Vehicles", null)
            .RequiredForUnlock = Fragments.SeaVoyagerFragment.SeaVoyagerFragmentTechType;

        prefab.SetGameObject(GetGameObjectAsync);

        prefab.Register();

        return this;
    }

    private IEnumerator GetGameObjectAsync(IOut<GameObject> returnedPrefab)
    {
        // Load the model
        var prefab = Object.Instantiate(Plugin.assetBundle.LoadAsset<GameObject>("SeaVoyagerPrefab"));
        prefab.SetActive(false);

        // Add essential components
        var identifier = prefab.AddComponent<PrefabIdentifier>();
        identifier.ClassId = Info.ClassID;
        prefab.AddComponent<TechTag>().type = Info.TechType;
        prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;

        // Load the rocket platform for reference. I only use it for the constructor animation and sounds.
        var rocketPlatformRequest = GetPrefabForTechTypeAsync(TechType.RocketBase);
        yield return rocketPlatformRequest;
        var rocketPlatformReference = rocketPlatformRequest.GetResult();

        // Get glass material
        Material glassMaterial = null;

        var aquariumTask = GetPrefabForTechTypeAsync(TechType.Aquarium);
        yield return aquariumTask;
        var aquariumModel = aquariumTask.GetResult();

        Renderer[] renderers = aquariumModel.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.name.ToLower().Contains("glass"))
                {
                    glassMaterial = material;
                    break;
                }
            }
        }

        // Load the base prefab
        var baseTask = PrefabDatabase.GetPrefabAsync("e9b75112-f920-45a9-97cc-838ee9b389bb");
        yield return baseTask;
        if (!baseTask.TryGetPrefab(out var basePrefab))
        {
            Plugin.Logger.LogError("Failed to load base prefab!");
            yield break;
        }

        // Apply materials
        MaterialUtils.ApplySNShaders(prefab, 5.5f, 1, 1, new LightmapMaterialModifier(1));
        var windowRenderer = prefab.SearchChild("Window").GetComponent<Renderer>();
        windowRenderer.material = glassMaterial;

        // Get the Transform of the models
        Transform interiorModels = Helpers.FindChild(prefab, "Interior").transform;
        Transform exteriorModels = Helpers.FindChild(prefab, "Exterior").transform;
        // Adds a Rigidbody. So it can move.
        var rigidbody = prefab.AddComponent<Rigidbody>();
        rigidbody.mass = 20000f; // Has to be really heavy. Measured in kilograms.
        rigidbody.useGravity = false; // Can't believe I didn't catch this one in the 1.0 version...
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // Basically an extension to Unity rigidbodys. Necessary for buoyancy.
        var worldForces = prefab.AddComponent<WorldForces>();
        worldForces.useRigidbody = rigidbody;
        worldForces.underwaterGravity =
            -5f; // Despite it being negative, which would apply downward force, this actually makes it go UP on the y axis.
        worldForces.aboveWaterGravity = 5f; // Counteract the strong upward force
        worldForces.waterDepth = -5f;
        var changeDepth = prefab.AddComponent<ChangeWaterDepth>();
        changeDepth.worldForces = worldForces;
        changeDepth.newDepth = 1;
        // Determines the places the little build bots point their laser beams at.
        var buildBots = prefab.AddComponent<BuildBotBeamPoints>();
        Transform beamPointsParent = Helpers.FindChild(prefab, "BuildBotPoints").transform;
        // These are arbitrarily placed.
        buildBots.beamPoints = new Transform[beamPointsParent.childCount];
        for (int i = 0; i < beamPointsParent.childCount; i++)
        {
            buildBots.beamPoints[i] = beamPointsParent.GetChild(i);
        }

        // The path the build bots take to get to the ship to construct it.
        Transform pathsParent = Helpers.FindChild(prefab, "BuildBotPaths").transform;
        // 4 paths, one for each build bot to take.
        CreateBuildBotPath(prefab, pathsParent.GetChild(0));
        CreateBuildBotPath(prefab, pathsParent.GetChild(1));
        CreateBuildBotPath(prefab, pathsParent.GetChild(2));
        CreateBuildBotPath(prefab, pathsParent.GetChild(3));
        // The effects for the constructor.
        var vfxConstructing = prefab.AddComponent<VFXConstructing>();
        var rocketPlatformVfx = rocketPlatformReference.GetComponentInChildren<VFXConstructing>();
        vfxConstructing.ghostMaterial = rocketPlatformVfx.ghostMaterial;
        vfxConstructing.surfaceSplashSound = rocketPlatformVfx.surfaceSplashSound;
        vfxConstructing.surfaceSplashFX = rocketPlatformVfx.surfaceSplashFX;
        vfxConstructing.Regenerate();
        // Don't want it tipping over...
        var stabilizer = prefab.AddComponent<Stabilizer>();
        stabilizer.uprightAccelerationStiffness = 40f;
        // Some components might need this. I don't WANT it to take damage though, so I will just give it a LOT of health.
        var liveMixin = prefab.AddComponent<LiveMixin>();
        var lmData = ScriptableObject.CreateInstance<LiveMixinData>();
        lmData.canResurrect = true;
        lmData.broadcastKillOnDeath = false;
        lmData.destroyOnDeath = false;
        lmData.invincibleInCreative = true;
        lmData.weldable = true;
        lmData.minDamageForSound = 20f;
        lmData.maxHealth = 2500;
        liveMixin.data = lmData;
        // I don't know if this does anything at all as ships float above the surface, but I'm keeping it.
        var oxygenManager = prefab.AddComponent<OxygenManager>();
        // I don't understand why I'm doing this, but I will anyway. The power cell is nowhere to be seen. To avoid learning how the EnergyMixin code works, I just added an external solar panel that stores all the power anyway.
        var energyMixin = prefab.AddComponent<EnergyMixin>();
        energyMixin.compatibleBatteries = new List<TechType>() { TechType.PowerCell, TechType.PrecursorIonPowerCell };
        energyMixin.defaultBattery = TechType.PowerCell;
        energyMixin.storageRoot = Helpers.FindChild(prefab, "StorageRoot").AddComponent<ChildObjectIdentifier>();
        // Allows power to connect to here.
        var powerRelay = prefab.AddComponent<PowerRelay>();
        // Necessary for SubRoot class Update behaviour so it doesn't return an error every frame.
        var lod = prefab.AddComponent<BehaviourLOD>();
        // Add VFXSurfaces to adjust footstep sounds. This is technically not necessary for the interior colliders, however.
        foreach (Collider col in prefab.GetComponentsInChildren<Collider>())
        {
            var vfxSurface = col.gameObject.AddComponent<VFXSurface>();
            vfxSurface.surfaceType = VFXSurfaceTypes.metal;
        }

        // Sky applier stuff.

        var interiorSky = Object
            .Instantiate(basePrefab.transform.Find("SkyBaseInterior").gameObject, prefab.transform, false)
            .GetComponent<Sky>();
        var glassSky = Object.Instantiate(basePrefab.transform.Find("SkyBaseGlass").gameObject, prefab.transform, false)
            .GetComponent<Sky>();

        var lightControl = prefab.AddComponent<LightingController>();
        lightControl.skies = new[]
        {
            new LightingController.MultiStatesSky
            {
                sky = interiorSky,
                masterIntensities = new[] { 1.3f, 1f, 0.5f },
                diffIntensities = new[] { 3, 1.5f, 1.2f },
                specIntensities = new[] { 1.4f, 0.6f, 0.5f },
                startDiffuseIntensity = 3,
                startMasterIntensity = 1.3f,
                startSpecIntensity = 1.4f
            },
            new LightingController.MultiStatesSky
            {
                sky = glassSky,
                masterIntensities = new[] { 0.73f, 0.4f, 0.4f },
                diffIntensities = new[] { 1.02f, 0.5f, 0.5f },
                specIntensities = new[] { 0.44f, 0.3f, 0.3f },
                startDiffuseIntensity = 1.02f,
                startMasterIntensity = 0.73f,
                startSpecIntensity = 0.44f
            }
        };
        lightControl.emissiveController = new LightingController.MultiStatesEmissive()
        {
            intensities = new[] { 1, 0.7f, 0f }
        };
        lightControl.fadeDuration = 0.3f;

        var lights = Helpers.FindChild(prefab, "LightsParent").GetComponentsInChildren<Light>();
        var multiStateLights = new MultiStatesLight[lights.Length];
        for (var i = 0; i < lights.Length; i++)
        {
            multiStateLights[i] = new MultiStatesLight()
            {
                intensities = new[] { 1, 0.5f, 0.05f },
                light = lights[i]
            };
        }

        lightControl.lights = multiStateLights;

        var interiorSkyApplier = interiorModels.gameObject.AddComponent<SkyApplier>();
        var interiorRenderersList = new List<Renderer>();
        interiorRenderersList.AddRange(interiorModels.GetComponentsInChildren<Renderer>());
        interiorRenderersList.AddRange(prefab.transform.Find("KeyPadDoor1").GetComponentsInChildren<Renderer>());
        interiorRenderersList.AddRange(prefab.transform.Find("KeyPadDoor2").GetComponentsInChildren<Renderer>());

        interiorSkyApplier.renderers = interiorRenderersList.ToArray();
        interiorSkyApplier.anchorSky = Skies.BaseInterior;
        interiorSkyApplier.lightControl = lightControl;
        interiorSkyApplier.lightControl = lightControl;

        var exteriorSkyApplier = prefab.AddComponent<SkyApplier>();
        exteriorSkyApplier.dynamic = true;
        var exteriorRenderersList = new List<Renderer>();
        exteriorRenderersList.AddRange(exteriorModels.GetComponentsInChildren<Renderer>());
        exteriorRenderersList.AddRange(prefab.transform.Find("Propeller").GetComponentsInChildren<Renderer>());
        exteriorRenderersList.AddRange(prefab.transform.Find("ExosuitDock").GetComponentsInChildren<Renderer>());
        exteriorRenderersList.AddRange(prefab.transform.Find("ExosuitDock2").GetComponentsInChildren<Renderer>());
        exteriorRenderersList.Remove(windowRenderer);
        exteriorSkyApplier.renderers = exteriorRenderersList.ToArray();

        var glassSkyApplier = prefab.AddComponent<SkyApplier>();
        glassSkyApplier.renderers = new[] { windowRenderer };
        glassSkyApplier.anchorSky = Skies.BaseGlass;
        glassSkyApplier.lightControl = lightControl;

        // Add entrance door
        var door = prefab.transform.Find("Model/Exterior/MainDoor/DoorCollider").gameObject;
        Plugin.Logger.LogDebug($"{door}");
        var hatch = door.AddComponent<SeaVoyagerDoor>();
        hatch.insideSpawn = prefab.transform.Find("Model/Exterior/MainDoor/EnterPosition").gameObject;
        hatch.outsideExit = prefab.transform.Find("Model/Exterior/MainDoor/ExitPosition").gameObject;
        hatch.enterCustomText = "SeaVoyager_Enter";
        hatch.exitCustomText = "SeaVoyager_Exit";
        hatch.ignoreObject = prefab;

        // Load a seamoth for reference
        var seamothRequest = GetPrefabForTechTypeAsync(TechType.Seamoth);
        yield return seamothRequest;
        GameObject seamothRef = seamothRequest.GetResult();

        // Get the seamoth's water clip proxy component. This is what displaces the water.
        var seamothProxy = seamothRef.GetComponentInChildren<WaterClipProxy>();
        // Find the parent of all the ship's clip proxys.
        Transform proxyParent = Helpers.FindChild(prefab, "ClipProxys").transform;

        // Loop through them all
        foreach (Transform child in proxyParent)
        {
            var waterClip = child.gameObject.AddComponent<WaterClipProxy>();
            waterClip.shape = WaterClipProxy.Shape.Box;
            // Apply the seamoth's clip material. No idea what shader it uses or what settings it actually has, so this is an easier option. Reuse the game's assets.
            waterClip.clipMaterial = seamothProxy.clipMaterial;
            // You need to do this. By default the layer is 0. This makes it displace everything in the default rendering layer. We only want to displace water.
            waterClip.gameObject.layer = seamothProxy.gameObject.layer;
        }

        // Arbitrary number. The ship doesn't have batteries anyway.
        energyMixin.maxEnergy = 1200f;

        // Add the SeaVoyager component. Inherits from SubRoot, the same component that both the cyclops submarine and bases use.
        var shipBehaviour = prefab.AddComponent<Mono.SeaVoyager>();
        shipBehaviour.worldForces = worldForces;
        shipBehaviour.LOD = lod;
        shipBehaviour.rb = rigidbody;
        shipBehaviour.powerRelay = powerRelay;
        shipBehaviour.isBase = true;
        shipBehaviour.isCyclops = true;
        shipBehaviour.modulesRoot = prefab.transform;
        shipBehaviour.oxygenMgr = oxygenManager;
        shipBehaviour.lightControl = lightControl;
        shipBehaviour.interiorSky = interiorSky;
        shipBehaviour.glassSky = glassSky;

        // It needs to produce power somehow
        var basePowerRelay = basePrefab.GetComponent<PowerRelay>();
        powerRelay.powerSystemPreviewPrefab = basePowerRelay.powerSystemPreviewPrefab;

        var powerCellsParent = new GameObject("PowerCellsParent").transform;
        powerCellsParent.SetParent(prefab.transform, false);
        powerCellsParent.localPosition = new Vector3(0, -15, -0.2f);
        powerCellsParent.localEulerAngles = Vector3.zero;
        powerCellsParent.gameObject.AddComponent<ChildObjectIdentifier>().ClassId = "SeaVoyagerPower";

        var placeholdersGroup = prefab.AddComponent<PrefabPlaceholdersGroupSafe>();
        placeholdersGroup.prefabIdentifier = identifier;
        var powerCellLocations = new[]
        {
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17, -0.5f),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17, -1),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17, -1.5f),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17, -2),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17, -2.5f),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17, -3),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17, -3.5f),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17.5f, -0.5f),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17.5f, -1),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17.5f, -1.5f),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17.5f, -2),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17.5f, -2.5f),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17.5f, -3),
            prefab.gameObject.transform.position + new Vector3(-1.7f, 17.5f, -3.5f)
        };

        var placeholders = new PrefabPlaceholder[powerCellLocations.Length];

        for (int i = 0; i < powerCellLocations.Length; i++)
        {
            var placeholder = new GameObject("PowerCellPlaceholder");
            var placeholderComponent = placeholder.AddComponent<PrefabPlaceholder>();
            placeholderComponent.prefabClassId = "0cb22d0e-ba5e-4e4b-b7a7-a67931fb5e0c";
            placeholder.transform.SetParent(powerCellsParent, false);
            placeholder.transform.localPosition = powerCellLocations[i];
            placeholder.transform.localRotation = Quaternion.identity;
            placeholders[i] = placeholderComponent;
        }

        placeholdersGroup.prefabPlaceholders = placeholders;

        // A ping so you can see it from far away
        var ping = prefab.AddComponent<PingInstance>();
        ping.pingType = Plugin.SeaVoyagerPingType;
        ping.origin = Helpers.FindChild(prefab, "PingOrigin").transform;

        // Add a respawn point
        Helpers.FindChild(prefab, "RespawnPoint").AddComponent<RespawnPoint>();

        // Motor sound
        var motorSound = Helpers.FindChild(prefab, "EngineLoop").AddComponent<ShipMotorSound>();
        motorSound.emitter = motorSound.gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
        motorSound.emitter.followParent = true;
        motorSound.seaVoyager = shipBehaviour;

        // Voice
        var shipVoice = prefab.AddComponent<ShipVoice>();
        var voiceSource = prefab.AddComponent<FMOD_CustomEmitter>();
        voiceSource.followParent = true;
        voiceSource.restartOnPlay = true;
        voiceSource.playOnAwake = false;
        // voiceSource.volume = Plugin.config.NormalizedAudioVolume;
        shipVoice.emitter = voiceSource;
        shipBehaviour.voice = shipVoice;

        // Shallow water scanner
        var shallowWaterScanner =
            Helpers.FindChild(prefab, "ShallowWaterScanner").AddComponent<ShipShallowWaterScanner>();
        shallowWaterScanner.seaVoyager = shipBehaviour;

        // Power depletion notification
        var shipPowerWarning = prefab.AddComponent<ShipPowerWarning>();
        shipPowerWarning.ship = shipBehaviour;

        // Auto-stop for when you fall off
        var autoStop = prefab.AddComponent<ShipAutoStop>();
        autoStop.ship = shipBehaviour;

        // Cool window in the bedroom
        var bedRoomWindow = Helpers.FindChild(prefab, "BedRoomWindow").AddComponent<BedRoomWindow>();
        bedRoomWindow.SetupPrefab(shipBehaviour);

        // Make sure you don't walk on the seafloor
        var walkableAreaBounds = prefab.AddComponent<ShipWalkableAreaBounds>();
        walkableAreaBounds.ship = shipBehaviour;

        // embark ladder
        var embarkLadder = Helpers.FindChild(prefab, "EmbarkLadder").AddComponent<ShipLadder>();
        embarkLadder.interactText = "Embark Sea Voyager";
        embarkLadder.SetAsMainEmbarkLadder(shipBehaviour);

        var embarkCinematic = Helpers.FindChild(prefab, "EmbarkCinematic").AddComponent<ShipCinematic>();
        embarkCinematic.Initialize("cyclops_ladder_long_up", "cinematic", 1.9f, ClimbUpLongSound,
            embarkLadder.transform.GetChild(0));

        embarkLadder.cinematic = embarkCinematic;

        // disembark ladder
        var disembarkLadder = Helpers.FindChild(prefab, "DisembarkLadder").AddComponent<ShipLadder>();
        disembarkLadder.interactText = "Disembark";

        // exit lower area ladder
        var exitLadder = Helpers.FindChild(prefab, "ExitLadder").AddComponent<ShipLadder>();
        exitLadder.interactText = "Ascend";

        var exitCinematic = Helpers.FindChild(prefab, "ExitCinematic").AddComponent<ShipCinematic>();
        exitCinematic.Initialize("cyclops_ladder_long_up", "cinematic", 1.9f, ClimbUpLongSound,
            exitLadder.transform.GetChild(0));

        exitLadder.cinematic = exitCinematic;

        // access cockpit loft ladder
        var loftLadder = Helpers.FindChild(prefab, "LoftLadder").AddComponent<ShipLadder>();
        loftLadder.interactText = "Ascend";

        var loftCinematic = Helpers.FindChild(prefab, "LoftLadderCinematic").AddComponent<ShipCinematic>();
        loftCinematic.Initialize("cyclops_ladder_short_up", "cinematic", 1f, ClimbUpShortSound,
            loftLadder.transform.GetChild(0));

        loftLadder.cinematic = loftCinematic;

        // access lower area ladder
        var descendLadder = Helpers.FindChild(prefab, "EntranceLadder").AddComponent<ShipLadder>();
        descendLadder.interactText = "Descend";

        var slideCinematic = Helpers.FindChild(prefab, "SlideDownCinematic").AddComponent<ShipCinematic>();
        slideCinematic.Initialize("rockethsip_cockpitLadderDown", "cinematic", 5f, SlideDownSound,
            descendLadder.transform.GetChild(0));

        descendLadder.cinematic = slideCinematic;

        // engine room ladder (up)
        var engineRoomLadderUp = Helpers.FindChild(prefab, "EngineRoomLadderUp").AddComponent<ShipLadder>();
        engineRoomLadderUp.interactText = "Ascend";

        var engineRoomUpCinematic = Helpers.FindChild(prefab, "EngineRoomUpCinematic").AddComponent<ShipCinematic>();
        engineRoomUpCinematic.Initialize("cyclops_ladder_short_up", "cinematic", 0.9f, ClimbUpShortSound,
            engineRoomLadderUp.transform.GetChild(0));

        engineRoomLadderUp.cinematic = engineRoomUpCinematic;

        // engine room ladder (down)
        var engineRoomLadderDown = Helpers.FindChild(prefab, "EngineRoomLadderDown").AddComponent<ShipLadder>();
        engineRoomLadderDown.interactText = "Descend";

        shipBehaviour.hud = Helpers.FindChild(prefab, "PilotCanvas").AddComponent<ShipHUD>();

        shipBehaviour.shipMotor = prefab.AddComponent<ShipMotor>();
        shipBehaviour.shipMotor.ship = shipBehaviour;

        shipBehaviour.propeller = Helpers.FindChild(prefab, "Propeller").AddComponent<ShipPropeller>();
        shipBehaviour.propeller.rotationDirection = new Vector3(0f, 0f, 1f);
        shipBehaviour.propeller.ship = shipBehaviour;

        shipBehaviour.voiceNotificationManager =
            Helpers.FindChild(prefab, "VoiceSource").AddComponent<VoiceNotificationManager>();
        shipBehaviour.voiceNotificationManager.subRoot = shipBehaviour;

        Helpers.FindChild(prefab, "KeyPadDoor1").AddComponent<ShipSlidingDoor>();
        Helpers.FindChild(prefab, "KeyPadDoor2").AddComponent<ShipSlidingDoor>();

        var dock1 = prefab.SearchChild("ExosuitDock").AddComponent<SuspendedDock>();
        dock1.ship = shipBehaviour;
        dock1.Initialize();

        var dock2 = prefab.SearchChild("ExosuitDock2").AddComponent<SuspendedDock>();
        dock2.ship = shipBehaviour;
        dock2.Initialize();

        shipBehaviour.skyraySpawner = prefab.SearchChild("SkyraySpawns").AddComponent<SkyraySpawner>();
        
        vfxConstructing.disableBehaviours = new List<Behaviour> { shipBehaviour };
        
        // Fix player flying or falling while inside the lower area
        var setKinematic = prefab.AddComponent<SetSeaVoyagerKinematic>();
        setKinematic.rb = rigidbody;
        setKinematic.voyager = shipBehaviour;
        setKinematic.maxInteriorYLevelMeasure = prefab.transform.Find("InteriorMaxYLevel");
        
        // FINALLY, return the prefab!
        returnedPrefab.Set(prefab);
    }

    private static void CreateBuildBotPath(GameObject gameObjectWithComponent, Transform parent)
    {
        var comp = gameObjectWithComponent.AddComponent<BuildBotPath>();
        comp.points = new Transform[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            comp.points[i] = parent.GetChild(i);
        }
    }
}