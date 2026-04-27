using System;
using UnityEngine;

namespace WeatherMod.Mono;

public class WeatherSoundUpdater : MonoBehaviour
{
    private WeatherEventAudio _audio;

    private State _currentState;
    
    private LoopingAudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = gameObject.EnsureComponent<LoopingAudioPlayer>();
    }

    public void SetActiveAudio(WeatherEventAudio audio)
    {
        if (_audio != audio)
            _audioPlayer.BeginPlayLoopingSound(null);

        _audio = audio;
    }
    
    private void Update()
    {
        if (_audio == null)
            return;
        
        _currentState = DetermineState();

        switch (_currentState)
        {
            case State.Underwater:
                _audioPlayer.BeginPlayLoopingSound(null);
                break;
            case State.SurfaceInside:
                if (_audio.InsideOnlyAmbience != null)
                    _audioPlayer.BeginPlayLoopingSound(_audio.InsideOnlyAmbience);
                break;
            case State.SurfaceOutside:
                if (_audio.NormalAmbience != null)
                    _audioPlayer.BeginPlayLoopingSound(_audio.NormalAmbience);
                break;
        }
    }

    private static State DetermineState()
    {
        if (MainCamera.camera.transform.position.y < Ocean.GetOceanLevel() + (Player.main.IsInside() ? -2f : 0f))
        {
            return State.Underwater;
        }

        return Player.main.IsInside() ? State.SurfaceInside : State.SurfaceOutside;
    }

    private enum State
    {
        SurfaceOutside,
        SurfaceInside,
        Underwater
    }
}