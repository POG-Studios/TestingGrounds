using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct ParentData : IComponentData
{
    public int randomNum;
    public Types.EntityType entityType;
    
}
