using UnityEngine;

namespace RobinPlayerModel;

public class DisableAdjustmentsDuringCinematics : MonoBehaviour
{
    public RuntimeBoneAdjuster[] adjusters;

    private bool _cinematicActive;
    private bool _cinematicActiveAndNotPiloting;
    private float _defaultNearClip;
    private float _cinematicNearClip = 0.15f;

    private void Start()
    {
        _cinematicActive = Player.main.cinematicModeActive;
        _cinematicActiveAndNotPiloting = Player.main.cinematicModeActive && !Player.main.isPiloting;
        _defaultNearClip = MainCamera.camera.nearClipPlane;
        UpdateState();
    }

    private void Update()
    {
        bool updateNeeded = false;
        
        bool state = Player.main.cinematicModeActive;
        if (state != _cinematicActive)
        {
            _cinematicActive = state;
            updateNeeded = true;
        }
        
        bool advancedState = Player.main.cinematicModeActive && !Player.main.isPiloting;
        if (advancedState != _cinematicActiveAndNotPiloting)
        {
            _cinematicActiveAndNotPiloting = advancedState;
            updateNeeded = true;
        }
        
        if (updateNeeded)
            UpdateState();
    }
    
    private void UpdateState()
    {
        foreach (var adjuster in adjusters)
        {
            adjuster.enabled = !_cinematicActive;
        }
        MainCamera.camera.nearClipPlane = _cinematicActiveAndNotPiloting ? _cinematicNearClip : _defaultNearClip;
    }
}