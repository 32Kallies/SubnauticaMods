using HarmonyLib;

namespace PdaTranslationMod;

[HarmonyPatch(typeof(PDALog))]
public static class PdaLogPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PDALog.Initialize))]
    private static void ReplaceMappings()
    {
        foreach (var logEntry in PDALog.mapping)
        {
            if (logEntry.Value == null) continue;
            if (Plugin.TranslatedDialogue.TryGetValue(logEntry.Key, out var translation))
            {
                logEntry.Value.sound = translation.asset;
            }
        }
    }
}