using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class PairParent : SystemBase
{
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.

        
    }

    public void SetParents()
    {
        Debug.Log("Setting parents");
        var entQuery = EntityManager.CreateEntityQuery(typeof(ParentTag)).ToEntityArray(Allocator.Temp);
        
        Entities.WithAll<SetParentTag>().ForEach((Entity ent, ref dataComponent dat) =>
        {
            var randIndex = Random.Range(0, entQuery.Length);
            
            dat.parentEnt = entQuery[randIndex];
            EntityManager.RemoveComponent<SetParentTag>(ent);
        }).WithStructuralChanges().WithoutBurst().Run();
    }
}
