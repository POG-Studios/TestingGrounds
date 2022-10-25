using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;

public partial class CreateBlobAssetSystem : SystemBase
{
    public BlobAssetReference<EntityAssetPool> blobRef;
    
    protected override void OnUpdate()
    {
        var numEntsInBlobArraryQueryDesc = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(BlobAssetPrefab) }
        };
        var numEntsQuery = GetEntityQuery(numEntsInBlobArraryQueryDesc);
        var numEnts = numEntsQuery.CalculateEntityCount();
        var ents = numEntsQuery.ToEntityArray(Allocator.Temp);
        
        var builder = new BlobBuilder(Allocator.Temp);
        ref EntityAssetPool entityAssetPool = ref builder.ConstructRoot<EntityAssetPool>();
        BlobBuilderArray<EntityAsset> arrayBuilder = builder.Allocate(ref entityAssetPool.entBlobArray, numEnts);

        for (int i = 0; i < ents.Length; i++)
        {
            arrayBuilder[i] = new EntityAsset { entAsset = ents[i] };
        }
        
        blobRef = builder.CreateBlobAssetReference<EntityAssetPool>(Allocator.Persistent);
        builder.Dispose();
        /*int index = 0;
        Entities.WithAll<BlobAssetPrefab>().ForEach((Entity ent) =>
        {

            arrayBuilder[index] = new EntityAsset
            {
                entAsset = ent
            };
        }).Run();*/
    }
}
