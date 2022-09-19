using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public ChildRaycastScript raycaster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Spawn();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            raycaster.CastRays();

        }
    }

    public void Spawn()
    {
        var blobs = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SetupBlobAssets>().blobArray;
        var ecbSystem =
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        var em = World.DefaultGameObjectInjectionWorld.EntityManager;

        List<Entity> tempList = new List<Entity>();

        var randmNum = Random.Range(0, 250);
        for (int i = 0; i < randmNum; i++)
        {
            var tempEnt = em.Instantiate(blobs[2]);

        }
        
        for (int i = 0; i < 5; i++)
        {
            var tempEnt = em.Instantiate(blobs[0]);
            var randX = Random.Range(0, 1000);
            var randZ = Random.Range(0, 1000);
            var randY = Random.Range(0, 1000);
            
            em.SetComponentData(tempEnt, new Translation{ Value = new float3(randX, 0, randZ)});
            //em.SetComponentData(tempEnt, new ParentData(){entityType = Types.EntityType.Parent, randomNum = randY});
            em.SetComponentData(tempEnt, new dataComponent(){entityType = Types.EntityType.Parent, intData = randY, pos = new int2(randX, randZ)});
            tempList.Add(tempEnt);
        }
        
        for (int i = 0; i < 50; i++)
        {
            var tempEnt = em.Instantiate(blobs[1]);
            var randX = Random.Range(0, 1000);
            var randZ = Random.Range(0, 1000);
            
            em.SetComponentData(tempEnt, new Translation{ Value = new float3(randX, 0, randZ)});

            em.AddComponent<ChildTag>(tempEnt);
            

            var tempComp = em.GetComponentData<dataComponent>(tempEnt);
            tempComp.pos = new int2(randX, randZ);
            tempComp.floatData = randX;
            tempComp.intData = Random.Range(0, 6900);
            tempComp.parentEnt = tempList[Random.Range(0, tempList.Count)];
            tempComp.entityType = Types.EntityType.Child;
            
            em.SetComponentData(tempEnt, tempComp);


        }
        

        //StartCoroutine(waiting());

    }

    IEnumerator waiting()
    {
        yield return null;
        
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PairParent>().SetParents();
    }
}
