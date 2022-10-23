using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[SaveComponent]
[EntityReference]
public struct dataComponent : IComponentData
{
    public int intData;
    public float floatData;
    public int2 pos;
    public Entity parentEnt;

    public bool newVariable;

    public Types.EntityType entityType;

    public int2 previousIndexNum;
    public int2 previousParentIndexNum;
    
    
}

public struct shitComponent : IComponentData
{
    public int shitData;
    public float shitFloat;
}

[SaveComponent]
public struct dataComponentBuffer : IBufferElementData
{
    public int bufferInt;
    public Entity bufferEnt;
}
