using ModStructureHelperPlugin.Mono;
using Nautilus.Commands;

namespace ModStructureHelperPlugin;

public static class Commands
{
    [ConsoleCommand("cleariconcache")]
    private static void ResetIconCache()
    {
        IconGenerator.ClearIconCache();
    }
}