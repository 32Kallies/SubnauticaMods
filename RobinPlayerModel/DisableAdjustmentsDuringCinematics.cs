using UnityEngine;

namespace RobinPlayerModel;

public class DisableAdjustmentsDuringCinematics : MonoBehaviour
{
    public RuntimeBoneAdjuster[] adjusters;

    private bool _cinematicActive;

    private void Start()
    {
        _cinematicActive = Player.main.cinematicModeActive;
        UpdateState();
    }

    private void Update()
    {
        bool state = Player.main.cinematicModeActive;
        if (state != _cinematicActive)
        {
            UpdateState();
        }
    }

    private void UpdateState()
    {
        foreach (var adjuster in adjusters)
        {
            adjuster.enabled = !_cinematicActive;
        }
    }
}