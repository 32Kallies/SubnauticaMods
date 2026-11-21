using BepInEx.Bootstrap;

namespace RobinPlayerModel.Compatibility;

public static class ThirdPersonCompat
{
    private static readonly string[] ThirdPersonMods =
    [
        "com.yw2theorycrafter.immersivethirdperson",
        "com.mikjaw.subnautica.thirdperson.mod"
    ];
    
    public static bool AnyThirdPersonModInstalled { get; private set; }

    public static void InitializeCompatibility()
    {
        foreach (var modGuid in ThirdPersonMods)
        {
            if (Chainloader.PluginInfos.ContainsKey(modGuid))
            {
                AnyThirdPersonModInstalled = true;
                return;
            }
        }
    }
}