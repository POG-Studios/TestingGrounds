using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public partial class TesterSystem : SystemBase
{
    protected override void OnUpdate()
    {
        /*Entities.WithAll<Enemy1Tag>().ForEach((Entity ent, in EnemyComponent ec) =>
        {

            Debug.Log($"enemy1 tag found {ent.Index}, {ent.Version}! int data {ec.intData}");
        }).Run();
        
        Entities.WithAll<Enemy2Tag>().ForEach((Entity ent, in EnemyComponent ec) =>
        {

            Debug.Log($"enemy2 tag found {ent.Index}, {ent.Version}! int data {ec.intData}");
        }).Run();*/
    }
}
