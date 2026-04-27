using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class Drizzle : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = Plugin.AssetBundle.LoadAsset<GameObject>("Weather_Drizzle");
    protected override float DestroyDelay { get; } = 5;
    protected override FogSettings Fog { get; } = new FogSettings();
    public override float MinDuration { get; } = 3 * 60;
    public override float MaxDuration { get; } = 4 * 60;
    public override WeatherEventAudio AmbientSound { get; } = new WeatherEventAudio(null, WeatherAudio.LightRainInsideLoop, null);
    public override int RainDropVfxEmission { get; } = 5;

    protected override void OnEventBegin(GameObject effectPrefab)
    {
        effectPrefab.AddComponent<AlignWithCamera>();
        foreach (var renderer in effectPrefab.GetComponentsInChildren<Renderer>())
        {
            WeatherMaterialUtils.ApplyRainMaterial(renderer);
        }

        effectPrefab.AddComponent<DisableWhenCameraUnderwater>();
        
        effectPrefab.AddComponent<WaterSplashVfxController>().affectedTransform =
            effectPrefab.transform.Find("WaterSplashes");
    }

    protected override void OnEventEnd(GameObject effectPrefab)
    {
        
    }
}