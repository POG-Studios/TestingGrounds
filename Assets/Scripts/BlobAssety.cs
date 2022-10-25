using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct EntityAsset
{
    public Entity entAsset;

    
}

public struct EntityAssetPool
{
    public BlobArray<EntityAsset> entBlobArray;
}


