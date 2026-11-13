using UnityEngine;

namespace RobinPlayerModel;

public class BoneAdjustment
{
    public Vector3 Position { get; init; }
    public AdjustmentMode PositionAdjustmentMode { get; init; }
    
    public Vector3 Rotation { get; init; }
    public AdjustmentMode RotationAdjustmentMode { get; init; }
}

public enum AdjustmentMode
{
    None,
    Offset,
    Replacement
}