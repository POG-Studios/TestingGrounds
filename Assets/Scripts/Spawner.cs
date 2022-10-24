using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{

    public int ParentCount;
    public int ChildCount;
    
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
        Debug.Log("Spawning...");
        var blobs = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SetupBlobAssets>().blobArray;
        
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;

        List<Entity> tempList = new List<Entity>();

        var randmNum = Random.Range(0, 250);
        for (int i = 0; i < randmNum; i++)
        {
            var tempEnt = em.Instantiate(blobs[2]);

        }
        
        for (int i = 0; i < ParentCount; i++)
        {
            var tempEnt = em.Instantiate(blobs[0]);
            var randX = Random.Range(0, 1000);
            var randZ = Random.Range(0, 1000);
            var randY = Random.Range(0, 1000);
            
        //    em.SetComponentData(tempEnt, new Translation{ Value = new float3(randX, 0, randZ)});
        
        
            em.SetComponentData(tempEnt, new PrefabKeyInfo() {prefabKey = 0});
            //em.SetComponentData(tempEnt, new ParentData(){entityType = Types.EntityType.Parent, randomNum = randY});
            em.SetComponentData(tempEnt, new dataComponent(){entityType = Types.EntityType.Parent, intData = randY, pos = new int2(randX, randZ), floatData = randX});
            em.SetComponentData(tempEnt, new EntID(){ ID = i});
            tempList.Add(tempEnt);
        }
        
        for (int i = 0; i < ChildCount; i++)
        {
            var tempEnt = em.Instantiate(blobs[1]);
            var randX = Random.Range(0, 1000);
            var randZ = Random.Range(0, 1000);
            
      //      em.SetComponentData(tempEnt, new Translation{ Value = new float3(randX, 0, randZ)});

            em.AddComponent<ChildTag>(tempEnt);
            

            var tempComp = em.GetComponentData<dataComponent>(tempEnt);
            tempComp.pos = new int2(randX, randZ);
            tempComp.floatData = randX;
            tempComp.intData = Random.Range(0, 6900);
       //     tempComp.parentEnt = tempList[Random.Range(0, tempList.Count)];
            tempComp.entityType = Types.EntityType.Child;
            
            em.SetComponentData(tempEnt, tempComp);

            em.SetComponentData(tempEnt, new PrefabKeyInfo() {prefabKey = 1});

            var tempComp2 = em.GetComponentData<shitComponent>(tempEnt);
            tempComp2.shitData = randX;
            tempComp2.shitFloat = randZ;
            
            em.SetComponentData(tempEnt, tempComp2);
            
            em.SetComponentData(tempEnt, new EntID(){ID = i + ParentCount});

            var randIndex = Random.Range(0, 10);
            var randIndexParent = Random.Range(0, tempList.Count);
            for (int j = 0; j < randIndex; j++)
            {
                var tempBuffer = em.GetBuffer<dataComponentBuffer>(tempEnt);
                tempBuffer.Add(new dataComponentBuffer() { bufferInt = randX, bufferEnt = tempList[randIndexParent]});
                //em.SetComponentData<dataComponentBuffer>(tempEnt);
            }


            if(i % 2 == 0)
                em.AddComponentData(tempEnt, new shitComponent() { });
        }
        

        StartCoroutine(waiting());

    }

    IEnumerator waiting()
    {
        yield return null;
        
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PairParent>().SetParents();
      //  var shit = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PairParent>();
    }
}
