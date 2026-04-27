using UnityEngine;
using WeatherMod.Mono;

namespace WeatherMod.WeatherEvents;

public class Foggy : WeatherEvent
{
    protected override GameObject EffectPrefab { get; } = null;
    protected override float DestroyDelay { get; } = 0;
    protected override FogSettings Fog { get; } = new FogSettings(0.002f, new Color(0.12f, 0.15f, 0.2f), 0.2f, 0.3f, 0.3f);
    public override float MinDuration { get; } = 40;
    public override float MaxDuration { get; } = 65;
    public override WeatherEventAudio AmbientSound { get; } = new WeatherEventAudio(WeatherAudio.WindyLoop, WeatherAudio.WindyLoopInside, null);

    protected override void OnEventBegin(GameObject effectPrefab) { }

    protected override void OnEventEnd(GameObject effectPrefab) { }
}