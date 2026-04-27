using Nautilus.Utility;

namespace WeatherMod;

public static class WeatherAudio
{
    private const string KSurfaceWeatherBuses = "bus:/master/SFX_for_pause/PDA_pause/all/SFX/creatures surface";
    private const string KInsideWeatherBuses = "bus:/master/SFX_for_pause/PDA_pause/all/indoorsounds";
    
    public static readonly ModSound[] ThunderSoundsNear = new[]
    {
        new ModSound("ThunderNear_1"),
        new ModSound("ThunderNear_2"),
        new ModSound("ThunderNear_3")
    };
    
    public static readonly ModSound[] ThunderSoundsFar = new[]
    {
        new ModSound("ThunderFar_1"),
        new ModSound("ThunderFar_2"),
        new ModSound("ThunderFar_3"),
        new ModSound("ThunderFar_4"),
        new ModSound("ThunderFar_5")
    };

    public static ModSound ThunderstormLoop { get; } = new ModSound("ThunderstormLoop4", "ThunderstormLoopOutdoors");
    public static ModSound ThunderstormLoopInside { get; } = new ModSound("ThunderstormLoop3", "ThunderstormLoopInside");
    public static ModSound WindyLoop { get; } = new ModSound("WindLoop_Heavy", "WindLoopOutdoors");
    public static ModSound WindyLoopInside { get; } = new ModSound("WindLoop_Heavy", "WindLoopInside");
    public static ModSound LightRainLoop { get; } = new ModSound("LightRainLoop2", "LightRainLoopOutdoors");
    public static ModSound LightRainInsideLoop { get; } = new ModSound("LightRainLoop_Covered", "LightRainLoopInside");
    public static ModSound GoldenThunderstormLoop { get; } = new ModSound("GoldenThunderstormLoop");
    
    public static void RegisterAll()
    {
        ThunderSoundsNear.ForEach(s => s.Register(KSurfaceWeatherBuses, true, 10, 400));
        ThunderSoundsFar.ForEach(s => s.Register(KSurfaceWeatherBuses, true, 40, 1000));
        
        ThunderstormLoop.Register(KSurfaceWeatherBuses, false, -1f, -1f);
        ThunderstormLoopInside.Register(KInsideWeatherBuses, false, -1f, -1f);
        WindyLoop.Register(KSurfaceWeatherBuses, false, -1f, -1f);
        WindyLoopInside.Register(KInsideWeatherBuses, false, -1f, -1f);
        LightRainLoop.Register(KSurfaceWeatherBuses, false, -1f, -1f);
        LightRainInsideLoop.Register(KInsideWeatherBuses, false, -1f, -1f);
        GoldenThunderstormLoop.Register(KSurfaceWeatherBuses, false, -1f, -1f);
    }
}