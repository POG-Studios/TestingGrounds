using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ParentData : IComponentData
{
    public int randomNum;
    public Types.EntityType entityType;
    
}
