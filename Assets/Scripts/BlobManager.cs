using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Unity.Entities;


public struct BlobManager : IComponentData
{
    public BlobAssetReference<BlobAssets> blobAssetReference;

    public Entity parent;
    public Entity child;
    public Entity dummy;
}

public struct BlobAssets
{
    public BlobArray<Entity> blobs;

}
