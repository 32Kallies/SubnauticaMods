using System;
using RobinPlayerModel.Compatibility;
using RobinPlayerModel.DataObjects;
using UnityEngine;

namespace RobinPlayerModel.MonoBehaviors;

public class RuntimeBoneAdjuster : MonoBehaviour
{
    public AdjustmentMode positionMode;
    public AdjustmentMode rotationMode;
    public Vector3 position;
    public Vector3 rotation;
    public Transform cameraPositionBone;
    public bool disableInThirdPerson;

    private bool _disableInThirdPersonThisSession;

    private const float ThirdPersonDisableSqrRange = 0.3f * 0.3f;

    private void Start()
    {
        _disableInThirdPersonThisSession = disableInThirdPerson && ThirdPersonCompat.AnyThirdPersonModInstalled;
    }

    private void LateUpdate()
    {
        if (_disableInThirdPersonThisSession && IsInThirdPerson())
            return;
        
        switch (positionMode)
        {
            case AdjustmentMode.Offset:
                transform.localPosition += position;
                break;
                case AdjustmentMode.Replacement:
                transform.localPosition = position;
                break;
        }

        switch (rotationMode)
        {
            case AdjustmentMode.Offset:
                transform.localRotation *= Quaternion.Euler(rotation);
                break;
            case AdjustmentMode.Replacement:
                transform.localRotation = Quaternion.Euler(rotation);
                break;
        }
    }

    private bool IsInThirdPerson()
    {
        var mainCamera = MainCamera.camera;
        if (mainCamera == null) return false;
        return Vector3.SqrMagnitude(mainCamera.transform.position - cameraPositionBone.position) > ThirdPersonDisableSqrRange;
    }
}