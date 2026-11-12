using Nautilus.Commands;

namespace TwistyBridgesObjectCleanser;

public static class Commands
{
    [ConsoleCommand("toggleshowtwistydeleters")]
    private static void ToggleShowTwistyDeleters()
    {
        DeleterVisualizer.ToggleShown();
    }
}