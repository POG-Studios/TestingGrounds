using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemyAuthoring : MonoBehaviour
{
    public int IntData;
    public float FloatData;
    public bool BoolData;

  //  public GameObject Prefab;
}

public struct EnemyComponent : IComponentData
{
    public int intData;
    public float floatData;
    public bool boolData;
    public PrefabTypes prefabType;
    
    public Entity entReference;

}

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {


        PrefabTypes tempType;
        var randIndex = Random.Range(0, 100);
        if (randIndex > 50)
        {
            AddComponent<Enemy1Tag>();
            tempType = PrefabTypes.Enemy1;
        }
        else
        {
            AddComponent<Enemy2Tag>();
            tempType = PrefabTypes.Enemy2;

        }
        
        AddComponent<BlobAssetPrefab>();
        
        AddComponent(new EnemyComponent
        {
            intData = authoring.IntData,
            floatData = authoring.FloatData,
            boolData = authoring.BoolData,
            prefabType = tempType,
            
            entReference = Entity.Null
        });

    }
}
