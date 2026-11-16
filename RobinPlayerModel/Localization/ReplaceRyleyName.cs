using Nautilus.Handlers;

namespace RobinPlayerModel.Localization;

// I'm well-aware this is overengineered for one single phrase in the entire game. 
public static class ReplaceRyleyName
{
    private static readonly string[] LinesMentioningRyley =
    [
        "EncyDesc_Aurora_DriveRoom_Terminal1"
    ];
    
    public static void ReplaceNameOfRyleyEverywhere()
    {
        if (Language.main == null)
        {
            Plugin.Logger.LogError("Language.main not found; cannot replace Ryley's name at this time.");
            return;
        }

        foreach (var line in LinesMentioningRyley)
        {
            var originalLine = Language.main.Get(line);
            if (string.IsNullOrEmpty(originalLine))
            {
                Plugin.Logger.LogWarning($"Failed to replace line {originalLine}");
                continue;
            }
            var newLine = originalLine.Replace("Ryley Robinson", "Robin Ayou");
            LanguageHandler.SetLanguageLine(line, newLine);
        }
    }
}