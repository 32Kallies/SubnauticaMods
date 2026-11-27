namespace RobinPlayerModel.DataObjects;

public sealed class FalseBone
{
    public required string BoneName { get; init; }
    public required string OriginalBoneName { get; init; }
    public required string[] RyleyMeshesToApplyTo { get; init; }
}