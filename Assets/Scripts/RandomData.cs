using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

public struct RandomData : IComponentData
{
    public Random Random;
    public int MinimumPosition;
    public int MaximumPosition;
    public int NextPosition => Random.NextInt(MinimumPosition, MaximumPosition);
}
public struct InitializeRandomDataTag : IComponentData{}