using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ParentAuthoring : MonoBehaviour
{
    public int IntData;
    public float FloatData;
    public PrefabTypes PrefabType;
}

public struct ParentComponent : IComponentData
{
    public int intData;
    public float floatData;
    public PrefabTypes prefabType;

}

public class ParentBaker : Baker<ParentAuthoring>
{
    public override void Bake(ParentAuthoring authoring)
    {
        AddComponent(new ParentComponent
        {
            intData = authoring.IntData,
            floatData = authoring.FloatData,
            prefabType = authoring.PrefabType
        });
        
        AddComponent<BlobAssetPrefab>();

    }
}
