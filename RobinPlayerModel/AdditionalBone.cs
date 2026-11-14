using UnityEngine;

namespace RobinPlayerModel;

public sealed class AdditionalBone
{
    public required string BoneName { get; init; }
    public required string Parent { get; init; }
    public required Vector3 LocalPosition { get; init; }
    public required Vector3 LocalRotation { get; init; }
}