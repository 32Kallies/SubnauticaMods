using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using ModStructureHelperPlugin.EntityHandling.Icons;
using ModStructureHelperPlugin.Utility;
using mset;
using UnityEngine;
using UWE;
using Object = System.Object;

namespace ModStructureHelperPlugin.Mono;

public class IconGenerator : MonoBehaviour
{
    public Camera cam;
    public GameObject sceneParent;
    public RenderTexture renderTexture;

    private static IconGenerator _instance;

    private static readonly Dictionary<string, EntityIcon> Icons = new();

    private static bool _spritesLoaded;
    private static Sprite _lightSprite;
    private static Sprite _particleSprite;
    private static Sprite _soundSprite;
    private static Sprite _atmosphereVolumeSprite;
    public static EntityIcon ErrorIcon { get; private set; }

    private static GameObject _skyPrefabForRendering;

    private const float TimeOutTime = 1f;

    private static readonly Color SoundColor = new Color(0.99f, 0.75f, 0.03f);

    private const string ShaderToIgnoreInBoundsCalc = "UWE/Particles/WBOIT-FakeVolumetricLight";

    public static bool TryGetIcon(string classId, out EntityIcon icon)
    {
        if (Icons.TryGetValue(classId, out icon)) return true;
        return false;
    }

    public static void ClearIconCache()
    {
        foreach (var icon in Icons.Values)
        {
            if (icon == null) continue;
            Destroy(icon.Sprite);
        }
        Icons.Clear();
    }

    public static bool HasIcon(string classId) => Icons.ContainsKey(classId);
    
    private static void LoadSprites()
    {
        // Load sprites
        _lightSprite = Plugin.AssetBundle.LoadAsset<Sprite>("Icon-Light");
        _soundSprite = Plugin.AssetBundle.LoadAsset<Sprite>("Icon-Audio");
        _particleSprite = Plugin.AssetBundle.LoadAsset<Sprite>("Icon-ParticleSystem");
        _atmosphereVolumeSprite = Plugin.AssetBundle.LoadAsset<Sprite>("Icon-AtmosphereVolume");
        ErrorIcon = new EntityIconBasic(Plugin.AssetBundle.LoadAsset<Sprite>("Icon-FailedToLoad"));
        _spritesLoaded = true;
    }

    public static IEnumerator GetIconForClassId(string classId, IconOutput output)
    {
        if (Icons.TryGetValue(classId, out var cachedIcon))
        {
            output.Icon = cachedIcon;
            yield break;
        }
        
        if (!_spritesLoaded)
            LoadSprites();

        if (_skyPrefabForRendering == null)
            yield return FetchSkyPrefab();

        yield return SafeGenerateIcon(GenerateIconForClassId(classId, output), classId, TimeOutTime, output);
    }

    private static IEnumerator GenerateIconForClassId(string classId, IconOutput output)
    {
        var task = PrefabDatabase.GetPrefabAsync(classId);
        if (task == null)
        {
            Plugin.Logger.LogWarning($"Skipping icon generation for prefab '{classId}' because its PrefabFactory is null!");
            yield break;
        }
        yield return task;
            
        if (!task.TryGetPrefab(out var prefab))
        {
            Plugin.Logger.LogWarning("Failed to get prefab for ClassId: " + classId);
            yield break;
        }

        if (prefab == null)
        {
            Plugin.Logger.LogWarning("Returned prefab is null for ClassID" + classId);
        }
        
        yield return GenerateIconForGameObject(prefab, classId, output);
    }
    
    private static IEnumerator GenerateIconForGameObject(GameObject prefab, string classId, IconOutput output)
    {
        EnsureIconGeneratorExists();

        if (Plugin.ModConfig.UseItemIconsInBrowser && CraftData.entClassTechTable.TryGetValue(classId, out var techType))
        {
            var itemSprite = SpriteManager.Get(techType, null);
            if (itemSprite != null)
            {
                var itemIcon = new EntityIconBasic(itemSprite);
                Icons[classId] = itemIcon;
                output.Icon = itemIcon;
                yield break;
            }
        }

        _instance.sceneParent.SetActive(true);
        var model = UWE.Utils.InstantiateDeactivated(prefab);
        ComponentStripUtils.StripComponents(model);
        foreach (var skyApplier in model.GetComponentsInChildren<SkyApplier>())
        {
            skyApplier.anchorSky = Skies.Custom;
            skyApplier.customSkyPrefab = _skyPrefabForRendering;
        }
        model.SetActive(true);

        if (TryUseSpecialIcon(model, out var specialIcon))
        {
            Icons[classId] = specialIcon;
            output.Icon = specialIcon;
            Destroy(model);
            yield break;
        }

        // Destroy model early on a 0.5-second timer in case something fails (to avoid populating the world)
        Destroy(model, 0.5f);
        model.transform.position = _instance.transform.position;
        var bounds = GetObjectBounds(model);
        var distance = Mathf.Clamp(Mathf.Max(bounds.extents.x, bounds.extents.z), 1, 60);
        _instance.cam.transform.position = bounds.center - _instance.cam.transform.forward * distance * 2;
        _instance.cam.farClipPlane = 200;
        
        // allow some limited initialization (for child prefab spawners)
        yield return null;

        var previousTarget = RenderTexture.active;
        var target = _instance.renderTexture;
        RenderTexture.active = target;
        
        Sprite sprite;
        try
        {
            sprite = GenerateIconSprite(target);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"Exception caught while generating icon for '{classId}': " + e);
            yield break;
        }
        finally
        {
            RenderTexture.active = previousTarget;
            Destroy(model);
        }
        
        if (_instance.sceneParent)
            _instance.sceneParent.SetActive(false);
        
        var icon = new EntityIconBasic(sprite);
        Icons[classId] = icon;
        output.Icon = icon;
    }

    private static void EnsureIconGeneratorExists()
    {
        if (_instance != null) return;
        
        // Create icon generator
        var obj = new GameObject("Icon Generator");
        _instance = obj.AddComponent<IconGenerator>();
            
        // Set up camera and render texture
        var camObj = new GameObject("Icon generator camera");
        camObj.SetActive(false);
        var camera = camObj.AddComponent<Camera>();
        camera.enabled = false;
        var renderTexture = new RenderTexture(256, 256, 32, RenderTextureFormat.Default);
        _instance.renderTexture = renderTexture;
            
        // Set camera settings
        camera.targetTexture = renderTexture;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.clear;
        camera.renderingPath = RenderingPath.DeferredShading;
        camera.allowMSAA = false;
        camera.cullingMask = 1711275799;
        camera.depthTextureMode = DepthTextureMode.Depth;
        camera.farClipPlane = 900;

        // Add WBOIT for transparency
        var mainCamera = MainCamera.camera;
        if (mainCamera != null)
        {
            var wboit = camera.gameObject.AddComponent<IconGeneratorWboit>();
            wboit.camera = camera;
            var originalWboit = MainCamera.camera.GetComponent<WBOIT>();
            wboit.compositeShader = originalWboit.compositeShader;
        }
        else
        {
            Plugin.Logger.LogError("MainCamera not found; cannot initialize WBOIT with the required materials");
        }
            
        camObj.SetActive(true);
            
        // Create scene
        var sceneParent = new GameObject("Scene parent");
        camObj.transform.parent = sceneParent.transform;
        camObj.transform.localEulerAngles = new Vector3(0, 210, 0);
        _instance.sceneParent = sceneParent;
        _instance.cam = camera;
        _instance.sceneParent.transform.parent = _instance.transform;
        _instance.transform.localPosition = new Vector3(0, 3000, 0);
    }

    private static Sprite GenerateIconSprite(RenderTexture target)
    {
        var tex = new Texture2D(target.width, target.height, TextureFormat.RGBA32, false, true);

        _instance.cam.Render();
        
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0, false);
        tex.Apply(false);
        
        var sprite = Sprite.Create(tex, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    private static IEnumerator SafeGenerateIcon(IEnumerator inner, string classId, float timeOutDuration, IconOutput output)
    {
        try
        {
            CoroutineHost.StartCoroutine(inner);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError("Exception caught while generating icon: " + e);
        }
        var endTime = Time.realtimeSinceStartup + timeOutDuration;
        while (Time.realtimeSinceStartup < endTime)
        {
            yield return null;
            if (output.Icon != null)
                yield break;
        }

        Icons[classId] = ErrorIcon;
        output.Icon = ErrorIcon;
    }

    private static IEnumerator FetchSkyPrefab()
    {
        var task = CraftData.GetPrefabForTechTypeAsync(TechType.Exosuit);
        yield return task;
        var exosuit = task.GetResult();
        var originalSky = exosuit.GetComponents<SkyApplier>().FirstOrDefault(s => s.customSkyPrefab != null);
        if (originalSky != null)
        {
            var skyPrefab = Instantiate(originalSky.customSkyPrefab);
            skyPrefab.name = "IconGeneratorSky";
            var sky = skyPrefab.GetComponent<Sky>();
            sky.affectedByDayNightCycle = false;
            _skyPrefabForRendering = skyPrefab;
        }
        else
        {
            Plugin.Logger.LogWarning("Could not find SkyApplier with interior sky prefab on Exosuit");
        }
    }

    private static bool TryUseSpecialIcon(GameObject gameObject, out EntityIcon specialIcon)
    {
        specialIcon = null;
        if (gameObject == null)
            return false;
        var renderers = gameObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0 || gameObject.GetComponent<VFXVolumetricLight>())
        {
            var light = gameObject.GetComponentInChildren<Light>();
            if (light != null)
            {
                specialIcon = new EntityIconBasic(_lightSprite, light.color.WithAlpha(1));
                return true;
            }
        }

        if (renderers.Length > 0 && renderers.All(r => r is ParticleSystemRenderer))
        {
            var particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
            var particleColor = Color.white;
            if (particleSystem != null)
            {
                particleColor = particleSystem.main.startColor.color.WithAlpha(1);
            }

            specialIcon = new EntityIconBasic(_particleSprite, particleColor);
            return true;
        }

        if (renderers.Length == 0)
        {
            // FMOD emitter icons
            if (gameObject.GetComponentInChildren<FMOD_CustomEmitter>() != null)
            {
                specialIcon = new EntityIconBasic(_soundSprite, SoundColor);
                return true;
            }

            // Atmosphere volume icons
            if (gameObject.TryGetComponent<AtmosphereVolume>(out var volume))
            {
                Color color = Color.white;
                
                // Attempt to get biome color for atmosphere volume
                var overrideBiome = volume.overrideBiome;
                var wbm = WaterBiomeManager.main;
                if (!string.IsNullOrEmpty(overrideBiome) && wbm != null)
                {
                    if (wbm.biomeLookup.TryGetValue(overrideBiome, out var biomeIndex) && biomeIndex != -1
                        && biomeIndex < wbm.biomeSettings.Count && wbm.biomeSettings[biomeIndex] != null
                        && wbm.biomeSettings[biomeIndex].settings != null)
                    {
                        Color rawColor;
                        var settings = wbm.biomeSettings[biomeIndex].settings;
                        // first check emissive color (this is the best measure)
                        if (settings.emissiveScale > Mathf.Epsilon && settings.emissive.maxColorComponent > Mathf.Epsilon)
                            rawColor = settings.emissive;
                        // next, check scattering color
                        else if (settings.scatteringColor.maxColorComponent > Mathf.Epsilon)
                            rawColor = settings.scatteringColor;
                        // finally fall back to White (Black is too dark)
                        else
                            rawColor = color;

                        Color.RGBToHSV(rawColor, out float h, out float s, out _);
                        color = Color.HSVToRGB(h, s, 1);
                    }
                }

                specialIcon = new EntityIconBasic(_atmosphereVolumeSprite, color);
                return true;
            }
        }

        return false;
    }

    private static Bounds GetObjectBounds(GameObject obj)
    {
        Bounds bounds = new Bounds(obj.transform.position, Vector3.one * 1f);
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            var material = renderer.sharedMaterial;
            if (material != null && material.shader != null && material.shader.name.Equals(ShaderToIgnoreInBoundsCalc))
            {
                continue;
            }

            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

    public class IconOutput
    {
        public EntityIcon Icon { get; set; }
    }
}