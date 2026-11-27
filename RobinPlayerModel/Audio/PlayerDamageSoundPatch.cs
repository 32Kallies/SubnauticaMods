using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;

namespace RobinPlayerModel.Audio;

[HarmonyPatch(typeof(PlayerDamageSounds))]
public static class PlayerDamageSoundPatch
{
    private static float _timeAnyNextSoundCanPlay;
    private static float _anySoundMinDelay = 0.5f;
    
    private static readonly PlayableSound SmokeDamage = new(AudioUtils.GetFmodAsset("event:/player/coughing"), 5);
    private static readonly PlayableSound PainNoTank = new(AudioUtils.GetFmodAsset("event:/player/Pain_no_tank"), 2);
    private static readonly PlayableSound PainNoTankLight = new(AudioUtils.GetFmodAsset("event:/player/Pain_no_tank_light"), 2);
    private static readonly PlayableSound PainSurface = new(AudioUtils.GetFmodAsset("event:/player/Pain_surface"), 1);
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerDamageSounds.OnTakeDamage))]
    private static bool OnTakeDamageReplacement(PlayerDamageSounds __instance, DamageInfo damageInfo)
    {
        if (!(damageInfo.damage > 0f) || damageInfo.type == DamageType.Radiation)
        {
            return true;
        }
        Player player = Player.main;
        var position = player.transform.position;
        if (damageInfo.type == DamageType.Smoke)
        {
            SmokeDamage.Play(position);
        }
        else if ((bool)player && !player.isUnderwater.value)
        {
            PainSurface.Play(position);
        }
        // skip the stupid Tank check
        else if (damageInfo.damage > 8f)
        {
            PainNoTank.Play(position);
        }
        else
        {
            PainNoTankLight.Play(position);
        }

        return false;
    }

    private class PlayableSound
    {
        private readonly FMODAsset _asset;
        private readonly float _minInterval;
        private float _timeNextPlay;

        public PlayableSound(FMODAsset asset, float minInterval)
        {
            _asset = asset;
            _minInterval = minInterval;
        }

        public void Play(Vector3 position)
        {
            if (Time.time < _timeNextPlay) return;
            if (Time.time < _timeAnyNextSoundCanPlay) return;
            _timeNextPlay = Time.time + _minInterval;
            _timeAnyNextSoundCanPlay = Time.time + _anySoundMinDelay;
            FMODUWE.PlayOneShot(_asset, position);
        }
    }
}