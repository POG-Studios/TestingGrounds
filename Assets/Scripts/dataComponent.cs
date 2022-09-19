using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct dataComponent : IComponentData
{
    public int intData;
    public float floatData;
    public int2 pos;
    public Entity parentEnt;

    public Types.EntityType entityType;

    public int2 previousIndexNum;
    public int2 previousParentIndexNum;
}
