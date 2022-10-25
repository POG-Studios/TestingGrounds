using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Spawner is called");
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            var spawnBlob = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CreateBlobAssetSystem>()
                .blobRef;
            
            var rand = Random.Range(25, 100);

            for (int i = 0; i < rand; i++)
            {
                var randIndex = Random.Range(0, spawnBlob.Value.entBlobArray.Length);
            
                var randx = Random.Range(5, 10);
                var randy = Random.Range(5, 10);

                var tempEnt = em.Instantiate(spawnBlob.Value.entBlobArray[randIndex].entAsset);
                em.SetComponentData(tempEnt, new LocalToWorldTransform{Value = new UniformScaleTransform{Position = new float3(randx, 0, randy)}});
            }
            
        }
    }
}
