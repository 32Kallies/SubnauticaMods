using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace WeatherMod;

[Menu("Epic Weather Mod")]
public class WeatherOptions : ConfigFile
{
    [Slider("Weather duration multiplier", 0.25f, 5, DefaultValue = 1f,
        Tooltip = "Scales the duration of each weather event. Higher values make the weather change less frequently.",
        Format = "{0:F2}x")]
    public float WeatherEventDurationMultiplier = 1;

    [Slider("Rain density multiplier", 1, 5, DefaultValue = 2f,
        Tooltip = "This setting controls the number of particles in the rain. Higher values WILL affect performance (also see the 'Rain has collisions' settings).", Format = "{0:F1}x", Step = 0.1f)]
    public float RainDensityMultiplier = 1.5f;
    
    [Slider("Screen shake strength", 0, 200, DefaultValue = 75,
        Tooltip = "Scales the screen shake with lightning effects", Format = "{0}%")]
    public int ScreenShakeMultiplier = 75;
    
    [Slider("Lightning chance", 0, 100, DefaultValue = 100,
        Tooltip = "Decrease this value to lower the chance of lighting strikes.", Format = "{0}%")]
    public int LightningStrikeChance = 100;
    
    [Toggle("Rain has collisions", Tooltip = "You may want to keep this disabled if you have issues with performance.")]
    public bool RainHasCollisions = false;

    [Toggle("???", Tooltip = "#$@!($*_1987??\nThis option does nothing (?)")]
    public bool Freddy = false;
}