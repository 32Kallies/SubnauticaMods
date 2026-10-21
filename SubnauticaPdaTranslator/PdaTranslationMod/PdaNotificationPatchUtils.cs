using System.Collections.Generic;

namespace PdaTranslationMod;

public static class PdaNotificationPatchUtils
{
    public static void ReplacePdaNotifications(IEnumerable<PDANotification> notifications)
    {
        foreach (var notification in notifications)
        {
            if (notification != null)
                ReplacePdaNotification(notification);
        }
    }
    
    public static void ReplacePdaNotification(PDANotification notification, string text = null)
    {
        if (string.IsNullOrEmpty(text))
            text = notification.text;
        if (string.IsNullOrEmpty(text))
        {
            Plugin.Logger.LogWarning("Could not translate notification " + notification);
            return;
        }
        if (Plugin.TranslatedDialogue.TryGetValue(text, out var newSound))
            notification.sound = newSound.asset;
    }
}