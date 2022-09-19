using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SetupBlobAssets : SystemBase
{
    private BlobManager blobManager;
    public NativeArray<Entity> blobArray;

    public NativeParallelHashMap<int, Entity> blobHashmap;

    protected override void OnStartRunning()
    {
        CreateNewBlobReference();
        
    }

    protected override void OnUpdate()
    {
        
    }

    public void CreateNewBlobReference()
    {

        Debug.Log("Creating blobs");
        blobArray = new NativeArray<Entity>(3, Allocator.Persistent);
        
        var blobManagerData = GetSingletonEntity<BlobManager>();
        blobManager = EntityManager.GetComponentData<BlobManager>(blobManagerData);
        
        using var blobBuilder = new BlobBuilder(Allocator.Temp);
        ref var blobAsset = ref blobBuilder.ConstructRoot<BlobAssets>();
        
        var miscBlobs = blobBuilder.Allocate(ref blobAsset.blobs, 3);
        blobHashmap = new NativeParallelHashMap<int, Entity>(3, Allocator.Persistent);

        miscBlobs[0] = blobManager.parent;
        blobArray[0] = miscBlobs[0];
        blobHashmap.Add(1, blobArray[0]);
        
        miscBlobs[1] = blobManager.child;
        blobArray[1] = miscBlobs[1];
        blobHashmap.Add(2, blobArray[1]);
        
        miscBlobs[2] = blobManager.dummy;
        blobArray[2] = miscBlobs[2];
        blobHashmap.Add(3, blobArray[2]);

        
        
        var blobManagerSingleton = GetSingleton<BlobManager>();
        blobManagerSingleton.blobAssetReference = blobBuilder.CreateBlobAssetReference<BlobAssets>(Allocator.Persistent);
        SetSingleton(blobManagerSingleton);
    }
}
