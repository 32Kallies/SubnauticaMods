namespace TwistyBridgesObjectCleanser;

public class DeleterGroup
{
    public required string Identifier { get; init; }
    public required string[] ClassIds { get; init; }
    public float[] Ranges { get; }

    public DeleterGroup(float range)
    {
        Ranges = new[] { range };
    }

    public DeleterGroup(params float[] ranges)
    {
        Ranges = ranges;
    }
}