using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Serialization.Json;
using Unity.Transforms;
using UnityEngine;
using Debug = UnityEngine.Debug;

[AlwaysUpdateSystem]
public partial class SaverSystem : SystemBase
{
    private string SavePath = Application.dataPath+"SaveData.json";

    protected override void OnUpdate()
    {
        var time = Stopwatch.StartNew();
        if (Input.GetKeyDown(KeyCode.S))
        {
            
            SaveGame();
            Debug.Log(time.Elapsed);
        }
        
        else if (Input.GetKeyDown(KeyCode.M))
        {
            LoadGame();
            Debug.Log(time.Elapsed);
        }
    }

    public void SaveGame()
    {
        var time = Stopwatch.StartNew();
        
        Debug.Log("Saving game");
     //   string potion = JsonUtility.ToJson(" ");

        var saveEnts = EntityManager.CreateEntityQuery(typeof(SaveTag)).ToEntityArray(Allocator.Temp);

        string data = "";

        var saveDataObj = new SaveDataObj();

        var saveList = new List<dataComponent>();
        var jsonList = new JsonArray();
        foreach (var ent in saveEnts)
        {
            var entData = EntityManager.GetComponentData<dataComponent>(ent);
            jsonList.Add(entData);
            saveList.Add(entData);

            entData.previousIndexNum = new int2(ent.Index, ent.Version);
            entData.previousParentIndexNum = new int2(entData.parentEnt.Index, entData.parentEnt.Version);
            saveDataObj.saveDat.Add(entData);
            //data = data + JsonUtility.ToJson(entData);
            // JsonArray
            //System.IO.File.AppendAllText("D:\\GameDev\\TestingGrounds\\Assets\\SaveData.json", data);

        }

        string temp = JsonUtility.ToJson(saveDataObj, true);
        System.IO.File.WriteAllText(SavePath, temp);
    
    }

    public void LoadGame()
    {
        Debug.Log("Loading game...");
        var FileData = System.IO.File.ReadAllText(SavePath);

        SaveDataObj saveData = JsonUtility.FromJson<SaveDataObj>(FileData);

        var remapper = new NativeParallelHashMap<int2, Entity>(500, Allocator.Temp);

        var blobHashmap = World.GetOrCreateSystem<SetupBlobAssets>().blobHashmap;

        foreach (var item in saveData.saveDat)
        {
            //Debug.Log($"entity position {item.pos}");

            var tempEnt = EntityManager.Instantiate(blobHashmap[(int)item.entityType]);
            EntityManager.SetComponentData(tempEnt, new Translation(){Value = new float3(item.pos.x, 0, item.pos.y)});
            EntityManager.SetComponentData(tempEnt, new dataComponent(){ entityType = item.entityType, pos = item.pos, floatData = item.floatData, intData = item.intData, parentEnt = item.parentEnt,previousIndexNum = item.previousIndexNum, previousParentIndexNum = item.previousParentIndexNum});
            if (item.entityType == Types.EntityType.Child)
            {
                EntityManager.AddComponent<ChildTag>(tempEnt);
            } 
            else if (item.entityType == Types.EntityType.Parent)
            {
                EntityManager.AddComponent<ParentTag>(tempEnt);
            }
            remapper.Add(item.previousIndexNum, tempEnt);
        }

        /*foreach (var a in saveData.saveDat)
        {
            var tempKey = new int2(a.parentEnt.Index, a.parentEnt.Version);
            var tempEnt = remapper[tempKey];
            
            
        }*/

        Entities.ForEach((Entity ent,ref dataComponent dat) =>
        {
            //Debug.Log("inside of loop");
            var tempKey = new int2(dat.previousParentIndexNum.x, dat.previousParentIndexNum.y);
            //Debug.Log(tempKey);
            if (tempKey.x != 0 && tempKey.y != 0)
            {
                var tempEnt = remapper[tempKey];
                dat.parentEnt = tempEnt;
            }
            

        }).Run();

        remapper.Dispose();
    }
}

public partial struct SavingJob : IJobEntity
{
    // Adds one to every translation component
    void Execute(ref dataComponent savDat)
    {
        
    }
}