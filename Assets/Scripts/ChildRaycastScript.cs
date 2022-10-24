using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ChildRaycastScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastRays()
    {
        Debug.Log("Casting Rays!");
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;

        var queryDesc = new EntityQueryDesc()
        {
            All = new ComponentType[] { typeof(ChildTag), typeof(dataComponent) }
        };

        var childQuery = em.CreateEntityQuery(queryDesc);
        var childArray = childQuery.ToEntityArray(Allocator.Temp);
        var childComponentArray = childQuery.ToComponentDataArray<dataComponent>(Allocator.Temp);

        for (int i = 0; i < childComponentArray.Length; i++)
        {
            Debug.Log($"PARENT ENT {childComponentArray[i].parentEnt}");
            var parentPos = em.GetComponentData<LocalToWorldTransform>(childComponentArray[i].parentEnt);
            
            Debug.DrawLine(new Vector3(childComponentArray[i].pos.x, 0, childComponentArray[i].pos.y), parentPos.Value.Position, Color.red, 5f);
        }
    }
}
