using System.Linq;
using Nautilus.FMod;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace RobinPlayerModel.Audio;

public static class FemaleSoundReplacements
{
    public static void ReplaceSounds()
    {
        var data = new SoundReplacementData(Plugin.SoundsBundle.LoadAsset<TextAsset>("sound-mapping").text);
        
        foreach (var entry in data.Entries)
        {
            RegisterSoundReplacementEntry(entry);
        }
    }

    private static void RegisterSoundReplacementEntry(SoundReplacementData.Entry entry)
    {
        int validClipCount = 0;
        foreach (var soundPath in entry.SoundClipNames)
        {
            if (!string.IsNullOrEmpty(soundPath))
            {
                validClipCount++;
            }
        }

        if (validClipCount == 0)
        {
            Plugin.Logger.LogWarning($"Skipping female sound replacement '{entry.DisplayName}' because it has 0 clips.");
            return;
        }

        AudioClip[] clips = new AudioClip[validClipCount];
        int i = 0;
        foreach (var clipName in entry.SoundClipNames)
        {
            if (string.IsNullOrEmpty(clipName))
                continue;
            
            clips[i] = Plugin.SoundsBundle.LoadAsset<AudioClip>(clipName);
            i++;
        }
        
        var fmodSounds = AudioUtils.CreateSounds(clips, AudioUtils.StandardSoundModes_2D).ToArray();
        var soundEvent = new FModMultiSounds(fmodSounds, AudioUtils.BusPaths.PlayerSFXs, true);
        CustomSoundHandler.RegisterCustomSound(entry.FmodPath, soundEvent);
    }
}