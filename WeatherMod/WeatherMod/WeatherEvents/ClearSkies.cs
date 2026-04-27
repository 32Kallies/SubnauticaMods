using UnityEngine;

namespace WeatherMod.WeatherEvents;

public class ClearSkies : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = null;
    protected override float DestroyDelay { get; } = 0;
    protected override FogSettings Fog { get; } = new FogSettings();
    public override float MinDuration { get; } = 3 * 60;
    public override float MaxDuration { get; } = 9 * 60;
    public override WeatherEventAudio AmbientSound { get; } = null;

    protected override void OnEventBegin(GameObject effectPrefab) { }

    protected override void OnEventEnd(GameObject effectPrefab) { }
}