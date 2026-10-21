using HarmonyLib;

namespace PdaTranslationMod;

[HarmonyPatch]
public static class NotificationPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Survival), nameof(Survival.Start))]
    private static void SurvivalPatch(Survival __instance)
    {
        PdaNotificationPatchUtils.ReplacePdaNotifications(__instance.foodWarningSounds);
        PdaNotificationPatchUtils.ReplacePdaNotifications(__instance.waterWarningSounds);
        PdaNotificationPatchUtils.ReplacePdaNotification(__instance.vitalsOkNotification);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LowOxygenAlert), nameof(LowOxygenAlert.Start))]
    private static void OxygenAlertsAndDepthWarningsPatch(LowOxygenAlert __instance)
    {
        foreach (var alert in __instance.alertList)
        {
            if (Plugin.TranslatedDialogue.TryGetValue(alert.notification.text, out var newSound))
                alert.soundSFX.SetAsset(newSound.asset);
        }

        var rebreatherWarnings = __instance.GetComponent<RebreatherDepthWarnings>();
        if (rebreatherWarnings == null)
        {
            Plugin.Logger.LogError("RebreatherDepthWarnings component not found on object " + __instance.gameObject);
            return;
        }

        foreach (var alert in rebreatherWarnings.alerts)
        {
            if (Plugin.TranslatedDialogue.TryGetValue(alert.alert.text, out var newSound))
                alert.alert.sound = newSound.asset;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(uGUI_PopupNotification), nameof(uGUI_PopupNotification.Show))]
    private static void KnownTechAlerts(ref uGUI_PopupNotification.Entry entry)
    {
        if (string.IsNullOrEmpty(entry.text))
            return;

        if (Plugin.TranslatedDialogue.TryGetValue(entry.text, out var newSound) && entry.sound != null)
            entry.sound = newSound.asset;
    }
}