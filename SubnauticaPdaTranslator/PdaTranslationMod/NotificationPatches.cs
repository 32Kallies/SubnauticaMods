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
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PDAEncyclopedia), nameof(PDAEncyclopedia.GetEntryData))]
    private static void PdaUnlockPatch(string key)
    {
        if (PDAEncyclopedia.mapping != null && PDAEncyclopedia.mapping.TryGetValue(key, out var entryData))
        {
            if (Plugin.TranslatedDialogue.TryGetValue("NewPdaData", out var newSound))
                entryData.sound = newSound.asset;
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ConstructorInput), nameof(ConstructorInput.StartUse))]
    private static void MobileVehicleBayPatch(ConstructorInput __instance)
    {
        if (Plugin.TranslatedDialogue.TryGetValue("InvalidConstructorPlacement", out var newSound))
            __instance.invalidNotification.sound = newSound.asset;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SignalPing), nameof(SignalPing.PlayVO))]
    private static void SignalUnlockPatch(SignalPing __instance)
    {
        if (Plugin.TranslatedDialogue.TryGetValue("NewSignalNotification", out var newSound))
            __instance.vo.sound = newSound.asset;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LeakingRadiation), nameof(LeakingRadiation.Start))]
    private static void AuroraRadiationPatch(LeakingRadiation __instance)
    {
        if (Plugin.TranslatedDialogue.TryGetValue("RemainingRadiationLeaks", out var leaksRemaining))
            __instance.leaksRemainingNotification.sound = leaksRemaining.asset;
        if (Plugin.TranslatedDialogue.TryGetValue("RadiationFixed", out var radiationFixed))
            __instance.leaksFixedNotification.sound = radiationFixed.asset;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LaunchRocket), nameof(LaunchRocket.Awake))]
    private static void LaunchRocketFailPatch(LaunchRocket __instance)
    {
        if (Plugin.TranslatedDialogue.TryGetValue("Rocket_ReadyGunStillActive", out var newSound))
            __instance.gunNotDisabled.sound = newSound.asset;
    }
}