using UnityEngine;

namespace RobinPlayerModel;

public class RuntimeBoneAdjuster : MonoBehaviour
{
    public AdjustmentMode positionMode;
    public AdjustmentMode rotationMode;
    public Vector3 position;
    public Vector3 rotation;

    private void LateUpdate()
    {
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
}