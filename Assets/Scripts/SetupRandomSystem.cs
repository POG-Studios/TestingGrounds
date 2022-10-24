using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public partial class SetUpRandomSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem _endSimECB;
    protected override void OnCreate()
    {
    //    _endSimECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        /*var ecbSim = _endSimECB.CreateCommandBuffer();
        Entities
            .WithAll<InitializeRandomDataTag>()
            .ForEach((Entity entity, ref RandomData randomData) =>
            {
                randomData.Random = Random.CreateFromIndex((uint)UnityEngine.Random.Range(0,100000));
                Debug.Log(randomData.NextPosition);
                EntityManager.SetComponentData(entity, new dataComponent(){ floatData =  randomData.NextPosition, pos = new int2(randomData.NextPosition, randomData.NextPosition), intData = randomData.NextPosition});
                
                EntityManager.RemoveComponent<InitializeRandomDataTag>(entity);
            }).WithStructuralChanges().WithoutBurst().Run();
        _endSimECB.AddJobHandleForProducer(this.Dependency);*/
    }
    
}