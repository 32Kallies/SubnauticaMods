using System.Collections;
using KallieʼsPropPack.MonoBehaviours;
using KallieʼsPropPack.PrefabLoading;
using Nautilus.Extensions;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;
using UWE;

namespace KallieʼsPropPack.Prefabs.Lab;

public class StaticLabProp : IEpicPrefabFactory
{
    private static readonly int GlowStrengthNight = Shader.PropertyToID("_GlowStrengthNight");
    private static readonly int GlowStrength = Shader.PropertyToID("_GlowStrength");
    private static readonly int Fresnel = Shader.PropertyToID("_Fresnel");
    private static readonly int IblReductionAtNight = Shader.PropertyToID("_IBLreductionAtNight");
    private static readonly int Shininess = Shader.PropertyToID("_Shininess");

    public IEnumerator BuildVariant(GameObject prefab, LoadedPrefabRegistrationData.Parameter[] parameters)
    {
        if (parameters == null) yield break;
        yield return ApplyBaseGameMaterials(prefab);
        yield return SetUpLights(prefab, parameters);
    }

    private IEnumerator ApplyBaseGameMaterials(GameObject prefab)
    {
        var name = prefab.name.ToLower();
        if (name.Contains("wall"))
        {
            if (name.Contains("door") && name.Contains("withframe"))
            {
                var doorFrameTask = PrefabDatabase.GetPrefabAsync("055b3160-f57b-46ba-80f5-b708d0c8180e");
                yield return doorFrameTask;
                if (!doorFrameTask.TryGetPrefab(out var doorFrame))
                {
                    Plugin.Logger.LogError("Failed to find door frame prefab!");
                    yield break;
                }
                var renderer = prefab.GetComponentInChildren<Renderer>();
                var materials = renderer.sharedMaterials;
                materials[1] = FixDarkMaterial(doorFrame.GetComponentInChildren<Renderer>().materials[0]);
                renderer.materials = materials;
            }
            else if (name.Contains("vent"))
            {
                var ventTask = PrefabDatabase.GetPrefabAsync("23a63afb-dbdd-4b34-b2f8-b423eb8d6f45");
                yield return ventTask;
                if (!ventTask.TryGetPrefab(out var wreckRoom01))
                {
                    Plugin.Logger.LogError("Failed to find ExplorableWreckRoom01 prefab!");
                    yield break;
                }

                var interiorRenderer = wreckRoom01.transform
                    .Find("explorable_wreckage_modular_room_01/room_01_interior").GetComponent<Renderer>();
                var renderer = prefab.GetComponentInChildren<Renderer>();
                var materials = renderer.materials;
                materials[1] = FixDarkMaterial(interiorRenderer.sharedMaterials[3]); // horizontal vent
                materials[2] = FixDarkMaterial(interiorRenderer.sharedMaterials[5]); // exterior
                materials[3] = FixDarkMaterial(interiorRenderer.sharedMaterials[4]); // alpha
                renderer.materials = materials;
            }
        }
    }

    private static Material FixDarkMaterial(Material original)
    {
        var material = new Material(original);
        material.SetFloat("_EmissionLM", 0);
        material.SetFloat("_EmissionLMNight", 0);
        material.SetFloat("_LightmapStrength", 0);
        material.color = Color.white;
        return material;
    }

    private IEnumerator SetUpLights(GameObject prefab, LoadedPrefabRegistrationData.Parameter[] parameters)
    {
        bool hasFlicker = false;
        bool isVolumetric = true;
        bool isOff = false;
        foreach (var param in parameters)
        {
            if (param.parameterName == "nonvolumetric")
            {
                isVolumetric = !param.GetValue<bool>();
            }

            if (param.parameterName == "flickering")
            {
                hasFlicker = param.GetValue<bool>();
            }

            if (param.parameterName == "off")
            {
                isOff = param.GetValue<bool>();
            }
        }

        if (isOff)
        {
            foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
            {
                var materials = renderer.materials;
                foreach (var material in materials)
                {
                    material.SetFloat(GlowStrength, 0);
                    material.SetFloat(GlowStrengthNight, 0);
                }
            }

            foreach (var light in prefab.GetComponentsInChildren<Light>(true))
            {
                light.enabled = false;
            }
        }
        else if (isVolumetric)
        {
            var lights = prefab.GetComponentsInChildren<Light>(true);
            bool hasPointLights = false;
            bool hasSpotLights = false;

            foreach (var light in lights)
            {
                if (light.type == LightType.Point)
                    hasPointLights = true;
                else if (light.type == LightType.Spot)
                    hasSpotLights = true;
            }

            VFXVolumetricLight volumetricSpotLight = null;
            VFXVolumetricLight volumetricPointLight = null;

            if (hasSpotLights)
            {
                var spotlightTask = PrefabDatabase.GetPrefabAsync("9912d6c1-f0fa-4a66-a827-4469539f5eb3");
                yield return spotlightTask;
                if (spotlightTask.TryGetPrefab(out var spotlightPrefab))
                {
                    volumetricSpotLight = spotlightPrefab.GetComponent<VFXVolumetricLight>();
                }
                else
                {
                    Plugin.Logger.LogError("Failed to load volumetric spotlight prefab");
                }
            }

            if (hasPointLights)
            {
                var pointLightTask = PrefabDatabase.GetPrefabAsync("c40b819e-3a2f-4fa3-a0c5-47f2191f5652");
                yield return pointLightTask;
                if (pointLightTask.TryGetPrefab(out var pointLightPrefab))
                {
                    // GetComponentInChildren because it is NOT on the root
                    volumetricPointLight = pointLightPrefab.GetComponentInChildren<VFXVolumetricLight>();
                }
                else
                {
                    Plugin.Logger.LogError("Failed to load volumetric point light prefab");
                }
            }

            foreach (var light in lights)
            {
                var referenceLight = light.type == LightType.Point ? volumetricPointLight : volumetricSpotLight;
                if (referenceLight == null)
                {
                    Plugin.Logger.LogWarning("Failed to load volumetric light for LightType: " + light.type);
                    continue;
                }
                var vfx = light.gameObject.AddComponent<VFXVolumetricLight>();
                vfx.volumGO = Object.Instantiate(referenceLight.transform.SearchChild("x_FakeVolumletricLight").gameObject,
                    light.transform);
                vfx.coneMat = referenceLight.coneMat;
                vfx.sphereMat = referenceLight.sphereMat;
                vfx.lightSource = light;
                vfx.lightType = light.type;
                vfx.syncMeshWithLight = true;
            }
        }

        if (hasFlicker && !isOff)
        {
            var flicker = prefab.AddComponent<LabLightFlicker>();
            flicker.renderers = prefab.GetComponentsInChildren<Renderer>();
            flicker.lights = prefab.GetComponentsInChildren<Light>(true);
        }
    }

    public MaterialModifier[] MaterialModifiers { get; } =
    {
        new DoubleSidedModifier(MaterialUtils.MaterialType.Cutout), new LabPropMaterialModifier()
    };

    private class LabPropMaterialModifier : MaterialModifier
    {
        public override void EditMaterial(Material material, Renderer renderer, int materialIndex, MaterialUtils.MaterialType materialType)
        {
            if (materialType == MaterialUtils.MaterialType.Transparent && renderer.gameObject.name.Contains("Glass"))
            {
                material.SetFloat(Fresnel, 0);
                material.SetFloat(Shininess, 7);
                material.SetFloat(IblReductionAtNight, 0.9f);
            }
            else if (material.name.Contains("Frame"))
            {
                material.SetFloat(Shininess, 6.5f);
                material.SetFloat(IblReductionAtNight, 0.97f);
            }
        }
    }
}