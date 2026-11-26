using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace TwistyBridgesPack;

public static class TwistyBridgesBiome
{
    private const string EncompassingString = "twistyBridges";
    private const string ShallowBiomeString = "twistyBridgesShallow";

    internal static void Register()
    {
        var sky = new BiomeHandler.SkyReference("SkySafeShallows", "SkyShallowTwistyBridges", _ => { });

        BiomeHandler.RegisterBiome(ShallowBiomeString, new WaterscapeVolume.Settings
            {
                absorption = new Vector3(100, 15, 3),
                scattering = 1.2f,
                // scatteringColor = new Color(140/255f, 255/255f, 208/255f, 1),
                scatteringColor = new Color(0.8f, 0.95f, 1),
                murkiness = 0.25f,
                emissive = new Color(0.24f, 0.9f, 0.886f, 1),
                emissiveScale = 0.004f,
                startDistance = 20,
                sunlightScale = 1.5f,
                ambientScale = 1,
                temperature = 16
            },
            sky);

        BiomeHandler.AddBiomeAmbience(ShallowBiomeString, AudioUtils.GetFmodAsset("event:/env/background/mushroom_background"),
            FMODGameParams.InteriorState.OnlyOutside);
        BiomeHandler.AddBiomeMusic(ShallowBiomeString, AudioUtils.GetFmodAsset("event:/env/music/reef_background_music"));

        var cubeAtmosphereVolume = new CustomPrefab(GetInfo("Shallow Twisty Bridges (Cube)"));
        cubeAtmosphereVolume.SetGameObject(new AtmosphereVolumeTemplate(cubeAtmosphereVolume.Info,
            AtmosphereVolumeTemplate.VolumeShape.Cube, ShallowBiomeString, 20));
        cubeAtmosphereVolume.Register();
        
        var sphereAtmosphereVolume = new CustomPrefab(GetInfo("Shallow Twisty Bridges (Sphere)"));
        sphereAtmosphereVolume.SetGameObject(new AtmosphereVolumeTemplate(sphereAtmosphereVolume.Info,
            AtmosphereVolumeTemplate.VolumeShape.Sphere, ShallowBiomeString, 20));
        sphereAtmosphereVolume.Register();
    }

    private static PrefabInfo GetInfo(string classId)
    {
        return new PrefabInfo(classId, $"{Plugin.RootFolderName}/{classId}Prefab", TechType.None);
    }
}