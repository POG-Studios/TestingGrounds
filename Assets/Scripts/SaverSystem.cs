using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Serialization.Json;
using Unity.Transforms;
using UnityEngine;
using Debug = UnityEngine.Debug;

[AlwaysUpdateSystem]
public partial class SaverSystem : SystemBase
{
    private string SavePath = Application.persistentDataPath+"\\SaveDataNEWBinaryFINAL.bin";

    protected override void OnUpdate()
    {
        var time = Stopwatch.StartNew();

        if (Input.GetKeyDown(KeyCode.S))
        {
            FinalSaver();           
            Debug.Log(time.Elapsed);

        }
        
        else if(Input.GetKeyDown(KeyCode.L))

        {
            FinalLoader();
            Debug.Log(time.Elapsed);

        }
    }

    public void SaveGameBinary()
    {
        Debug.Log("Saving the game in binary!");
        using (BinaryWriter writer = new BinaryWriter(File.Open(SavePath, FileMode.OpenOrCreate)))
        {
            foreach (var type in TypeManager.AllTypes)
            {                      
                //Debug.Log($"Type {type}   {type.TypeIndex}");

                if (type.Category == TypeManager.TypeCategory.ComponentData)
                {
                    
                    if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                    {
                        /*ar typeIndex = type.TypeIndex;
                        var typeInfo = TypeManager.GetTypeInfo(typeIndex).Type;*/
                    
                        using var query = EntityManager.CreateEntityQuery(type.Type);
                        using var queryEntities = query.ToEntityArray(Allocator.Temp);
                        
                        var tempComp = GetDynamicComponentTypeHandle(type.Type);
                        using var chunks = query.CreateArchetypeChunkArray(Allocator.Temp);
                        
                        var entityHandle = GetEntityTypeHandle();
                        
                        writer.Write(query.CalculateEntityCount());

                        var tempLength = type.ElementSize;
                        var numEntities = query.CalculateEntityCount();
                        
                        writer.Write(tempLength);
                        writer.Write(numEntities);

                        foreach (var chunk in chunks)
                        {
                            //var entityList = new NativeArray<Entity>(chunk.ChunkEntityCount, Allocator.Temp);
                            var entities = chunk.GetNativeArray(entityHandle);
                            var components = chunk.GetDynamicComponentDataArrayReinterpret<byte>(tempComp, type.ElementSize);
                            
                            if (components.Length == 0) continue;
                            
                            var componentLength = components.Length / chunk.ChunkEntityCount;


                            for (int i = 0; i < chunk.ChunkEntityCount; i++)
                            {
                                writer.Write(entities[i].Index);
                                writer.Write(entities[i].Version);
                                
                                //writer.Write();
                                
                                writer.Write(components.Slice(i * tempLength, tempLength).ToArray());

                            }

                            /*var chunkStruct = new ChunkSaveStruct(componentLength, entities.Length, components.Length);

                            for (int i = 0; i < entities.Length; i++)
                            {
                                chunkStruct.entityArray[i] = entities[i];
                            }
                            //writer.Write(entities.Reinterpret<byte>());

                            for (int i = 0; i < components.Length; i++)
                            {
                                //writer.Write(components[i]);
                                chunkStruct.componentData[i] = components[i];
                            }*/
                            
                            //Marshal.StructureToPtr(chunkStruct, IntPtr.Zero, false);
                        }

                        
                        // var saver = new ComponentSave(this, typeIndex);
                       
                        Debug.Log($"Type {type.Type}  {type.TypeIndex}");
                        Debug.Log($"saved game to: {SavePath}");


                        /*var query = EntityManager.CreateEntityQuery(typeof(type.Type));
                           
                        var tempComp = GetDynamicComponentTypeHandle(type.Type);
                        
                        var query2 = EntityManager.CreateEntityQuery(typeof(tempComp));*/


                    }
                }
            }
        }
        
    }
    
      public void SaveGameBinaryFormatter()
    {
        Debug.Log("Saving the game in binaryFormatter!");

        var binFormatter = new BinaryFormatter();
        var writer = new BinaryWriter(File.Open(SavePath, FileMode.OpenOrCreate));
        //var stream
            foreach (var type in TypeManager.AllTypes)
            {                      
                //Debug.Log($"Type {type}   {type.TypeIndex}");

                
                if (type.Category == TypeManager.TypeCategory.ComponentData)
                {
                    
                    if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                    {
                        /*ar typeIndex = type.TypeIndex;
                        var typeInfo = TypeManager.GetTypeInfo(typeIndex).Type;*/
                    
                        using var query = EntityManager.CreateEntityQuery(type.Type);
                        using var queryEntities = query.ToEntityArray(Allocator.Temp);
                        
                        var tempComp = GetDynamicComponentTypeHandle(type.Type);
                        using var chunks = query.CreateArchetypeChunkArray(Allocator.Temp);
                        
                        var entityHandle = GetEntityTypeHandle();

                        /*
                        var headerStream = new MemoryStream();
                        
                        binFormatter.Serialize(headerStream, query.CalculateEntityCount());*/
                        writer.Write(query.CalculateEntityCount());

                        var tempComponentLength = type.ElementSize;
                        var numEntities = query.CalculateEntityCount();
                        
                        
                        writer.Write(type.TypeIndex);
                        writer.Write(tempComponentLength);
                        writer.Write(numEntities);
                        /*binFormatter.Serialize(headerStream, numEntities);

                        binFormatter.Serialize(headerStream, tempLength);

                        writer.Write(headerStream.ToArray());*/

                        //var dataStream = new MemoryStream();
                       //Debug.Log($"numEntities: {numEntities}  tempLength: {tempComponentLength}  ");

                       //var tempTemp = chunks.Reinterpret<byte>();

                       var chunksData = chunks.Reinterpret<byte>();

                       writer.Write(chunksData.Length);

                        foreach (var chunk in chunks)
                        {
                            //var entityList = new NativeArray<Entity>(chunk.ChunkEntityCount, Allocator.Temp);
                            var entities = chunk.GetNativeArray(entityHandle);
                            var components = chunk.GetDynamicComponentDataArrayReinterpret<byte>(tempComp, type.ElementSize);
                            
                            
                            //var tempTemp = chunks.
                            //var components2 = chunk.compo
                            
                          
                            if (components.Length == 0) continue;
                            
                            var componentLength = components.Length / chunk.ChunkEntityCount;


                            for (int i = 0; i < chunk.ChunkEntityCount; i++)
                            {
                                /*
                                binFormatter.Serialize(dataStream, entities[i].Index);
                                binFormatter.Serialize(dataStream, entities[i].Version);
                                binFormatter.Serialize(dataStream, components.Slice(i * tempLength, tempLength).ToArray());
                                */
                                
                                
                                
                                writer.Write(entities[i].Index);
                                writer.Write(entities[i].Version);
                                
                                /*Debug.Log($"length of dataComponent: {dataStream.Length}");
                                Debug.Log($"length of slice: {components.Slice(i * tempComponentLength, tempComponentLength).Length}");*/
                                //writer.Write(dataStream.ToArray());
                                
                                //writer.Write(components.Slice(i * tempComponentLength, tempComponentLength).ToArray());

                            }
                            
                            writer.Write(chunksData.ToArray());

                            /*var chunkStruct = new ChunkSaveStruct(componentLength, entities.Length, components.Length);

                            for (int i = 0; i < entities.Length; i++)
                            {
                                chunkStruct.entityArray[i] = entities[i];
                            }
                            //writer.Write(entities.Reinterpret<byte>());

                            for (int i = 0; i < components.Length; i++)
                            {
                                //writer.Write(components[i]);
                                chunkStruct.componentData[i] = components[i];
                            }*/
                            
                            //Marshal.StructureToPtr(chunkStruct, IntPtr.Zero, false);
                        }

                        
                        // var saver = new ComponentSave(this, typeIndex);
                       
                        /*
                        Debug.Log($"Type {type.Type}  {type.TypeIndex}");
                        Debug.Log($"saved game to: {SavePath}");
                        */


                        /*var query = EntityManager.CreateEntityQuery(typeof(type.Type));
                           
                        var tempComp = GetDynamicComponentTypeHandle(type.Type);
                        
                        var query2 = EntityManager.CreateEntityQuery(typeof(tempComp));*/


                    }
                }
            
        }
        
    }


       public void SaveGameBinaryArray()
    {
        Debug.Log("Saving the game in binaryArray!");

        var writer = new BinaryWriter(File.Open(SavePath, FileMode.OpenOrCreate));
            foreach (var type in TypeManager.AllTypes)
            {                      
                //Debug.Log($"Type {type}   {type.TypeIndex}");

                
                if (type.Category == TypeManager.TypeCategory.ComponentData)
                {
                    
                    if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                    {
                        var queryDesc = new EntityQueryDesc()
                        {
                            All = new ComponentType[]
                            {
                                type.Type,
                                typeof(PrefabKeyInfo)
                            }
                        };
                        
                        using var query = EntityManager.CreateEntityQuery(queryDesc);
                        using var queryEntities = query.ToEntityArray(Allocator.Temp);
                        
                        var tempComp = GetDynamicComponentTypeHandle(type.Type);
                        using var chunks = query.CreateArchetypeChunkArray(Allocator.Temp);

                        using var prefabKeyInfo = query.ToComponentDataArray<PrefabKeyInfo>(Allocator.Temp);
                        
                        var entityHandle = GetEntityTypeHandle();
                        
                        writer.Write(query.CalculateEntityCount());

                        var tempComponentLength = type.ElementSize;
                        var numEntities = query.CalculateEntityCount();
                        
                        
                        writer.Write(type.TypeIndex);
                        writer.Write(tempComponentLength);
                        writer.Write(numEntities);

                        var chunksData = chunks.Reinterpret<byte>();

                       writer.Write(chunksData.Length);

                       int tempIndex = 0;
                        foreach (var chunk in chunks)
                        {
                            
                            var entities = chunk.GetNativeArray(entityHandle);
                            var componentBytes = chunk.GetDynamicComponentDataArrayReinterpret<byte>(tempComp, type.ElementSize);
                            //var components = chunk.GetChunkComponentData<PrefabKeyInfo>();

                            if (componentBytes.Length == 0) continue;
                            
                            var componentLength = componentBytes.Length / chunk.ChunkEntityCount;


                            for (int i = 0; i < chunk.ChunkEntityCount; i++)
                            {

                                writer.Write(entities[i].Index);
                                writer.Write(entities[i].Version);
                                writer.Write(prefabKeyInfo[tempIndex].prefabKey);
                                tempIndex++;
                                
                                //writer.Write(typeof(ChunkEntitiesDescription));
                            }
                            
                            writer.Write(chunksData.ToArray());
                            
                        }
                    }
                }
            }
    }

       public unsafe void PasterSaver()
       {
           Debug.Log("Calling paster tester");
           var queryDesc = new EntityQueryDesc()
           {
               All = new ComponentType[]
               {
                   typeof(dataComponent),
                   typeof(PrefabKeyInfo)
               }
           };
           var type = typeof(dataComponent);

           var typeIndex = TypeManager.GetTypeIndex(typeof(dataComponent));
           var typeInfo = TypeManager.GetTypeInfo(typeIndex);
                        
           using var query = EntityManager.CreateEntityQuery(queryDesc);
           //using var queryEntities = query.ToEntityArray(Allocator.Temp);
                        
           var typeHandle = GetDynamicComponentTypeHandle(type);
           using var chunks = query.CreateArchetypeChunkArray(Allocator.Temp);
           
           var writer = new BinaryWriter(File.Open(SavePath, FileMode.OpenOrCreate));

           var output = new NativeArray<byte>(512, Allocator.Temp);
           
           writer.Write(query.CalculateEntityCount());
           writer.Write(typeIndex);


           var entHandle = GetEntityTypeHandle();

           for (int i = 0; i <1; i++)
           {
               var chunk = chunks[i];
               //var ents = chunk.GetNativeArray(entHandle);
               
               var componentBytes = chunk.GetDynamicComponentDataArrayReinterpret<byte>(typeHandle, typeInfo.ElementSize);
               writer.Write(componentBytes.Length);
               writer.Write(componentBytes.ToArray());
               //var scrPtr = componentBytes.GetUnsafePtr();
               /*var componentBytes = chunk.GetDynamicComponentDataArrayReinterpret<byte>(tempComp, typeInfo.ElementSize);
               var scrPtr = componentBytes.GetUnsafePtr();
               var dstPtr = output.GetUnsafePtr();
               
               UnsafeUtility.MemCpy(dstPtr, scrPtr, componentBytes.Length);*/


           }

           for (int i = 0; i < output.Length; i++)
           {
               Debug.Log(output[i]);
           }

           output.Dispose();
           
           using var prefabKeyInfo = query.ToComponentDataArray<PrefabKeyInfo>(Allocator.Temp);
           
           var tempComponentLength = typeInfo.ElementSize;
           var numEntities = query.CalculateEntityCount();
       }

       public unsafe void PasterLoader()
       {
           Debug.Log("CallingPaster loader");
           var reader = new BinaryReader(File.Open(SavePath, FileMode.OpenOrCreate));

           var numEnts = reader.ReadInt32();
           var typeIndex = reader.ReadInt32();
           var byteArrayLength = reader.ReadInt32();
           
           var blobHashmap = World.GetOrCreateSystem<SetupBlobAssets>().blobHashmap;

           for (int i = 0; i < numEnts; i++)
           {
               EntityManager.Instantiate(blobHashmap[1]);
           }

           var data = reader.ReadBytes(byteArrayLength);
           var data2 = new NativeArray<byte>(data.Length, Allocator.Temp);
           
           for (int i = 0; i < data.Length; i++)
           {
               data2[i] = data[i];
           }
           
           
           var queryDesc = new EntityQueryDesc()
           {
               All = new ComponentType[]
               {
                   typeof(dataComponent),
                   typeof(PrefabKeyInfo)
               }
           };
           //var type = typeof(dataComponent);

           //var typeIndex = TypeManager.GetTypeIndex(typeof(dataComponent));
           var type = TypeManager.GetType(typeIndex);
           var typeInfo = TypeManager.GetTypeInfo(typeIndex);
                        
           using var query = EntityManager.CreateEntityQuery(queryDesc);
           //using var queryEntities = query.ToEntityArray(Allocator.Temp);
                        
           var tempComp = GetDynamicComponentTypeHandle(type);
           using var chunks = query.CreateArchetypeChunkArray(Allocator.Temp);
           
           var componentBytes = chunks[0].GetDynamicComponentDataArrayReinterpret<byte>(tempComp, typeInfo.ElementSize);
           
           

           var scrPtr = data2.GetUnsafePtr();
           var dstPtr = componentBytes.GetUnsafePtr();
               
           UnsafeUtility.MemCpy(dstPtr, scrPtr, componentBytes.Length);
       }

       public unsafe void FinalSaver()
       {
           Debug.Log("Saving the game Finally!");

           var writer = new BinaryWriter(File.Open(SavePath, FileMode.OpenOrCreate));

           var saveTagQuery = EntityManager.CreateEntityQuery(typeof(SaveTag));
           var totalNumEnts = saveTagQuery.CalculateEntityCount();
           
           writer.Write(69420); // placeholder mapGen seed
           
           writer.Write(totalNumEnts);

           int numComponents = 0;
           
           foreach (var type in TypeManager.AllTypes)
           {
               if (type.Category == TypeManager.TypeCategory.ComponentData || type.Category == TypeManager.TypeCategory.BufferData)
               {
                   if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                   {
                       numComponents++;
                   }
               }
           }
           writer.Write(numComponents);
           
           foreach (var type in TypeManager.AllTypes)
           {
               if (type.Category == TypeManager.TypeCategory.ComponentData)
               {
                   if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                   {
                       var saveQueryDesc = new EntityQueryDesc()
                       {
                           All = new ComponentType[]
                           {
                               type.Type,
                               typeof(PrefabKeyInfo)
                           }
                       };

                       var typeHandle = EntityManager.GetDynamicComponentTypeHandle(type.Type);

                       var saveQuery = EntityManager.CreateEntityQuery(saveQueryDesc);

                       var saveQueryEnts = saveQuery.ToEntityArray(Allocator.Temp);

                       var saveQueryEntKeys = saveQuery.ToComponentDataArray<PrefabKeyInfo>(Allocator.Temp);
                       
                       var saveQueryChunks = saveQuery.CreateArchetypeChunkArray(Allocator.Temp);
                       
                       //this is the component header
                       writer.Write(type.StableTypeHash);
                       writer.Write(type.TypeIndex);
                       writer.Write(type.ElementSize);
                       writer.Write(saveQueryChunks.Length);
                       writer.Write(saveQueryEnts.Length);

                       // save the data for all Entities associated with this component
                       for (int i = 0; i < saveQueryEnts.Length; i++)
                       {
                           writer.Write(saveQueryEnts[i].Index);
                           writer.Write(saveQueryEnts[i].Version);
                           writer.Write(saveQueryEntKeys[i].prefabKey);
                       }

                       //save the component data for all Entities in the chunks
                       for (int i = 0; i < saveQueryChunks.Length; i++)
                       {
                           var chunk = saveQueryChunks[i];
               
                           var componentBytes = chunk.GetDynamicComponentDataArrayReinterpret<byte>(typeHandle, type.ElementSize);
                           
                           writer.Write(componentBytes.Length);
                           writer.Write(componentBytes.ToArray());
                           
                           componentBytes.Dispose();
                       }
                       
                       saveQuery.Dispose();
                       saveQueryEnts.Dispose();
                       saveQueryEntKeys.Dispose();
                       saveQueryChunks.Dispose();
                       
                   }
               }
               
               else if (type.Category == TypeManager.TypeCategory.BufferData)
               {
                   if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                   {
                       var queryDesc = new EntityQueryDesc()
                       {
                           All = new ComponentType[]
                           {
                               type.Type,
                               typeof(PrefabKeyInfo)
                           }
                       };
                       
                       
                       using var query = EntityManager.CreateEntityQuery(queryDesc);
                       using var saveQueryEnts = query.ToEntityArray(Allocator.Temp);
                        
                       var tempComp = GetDynamicComponentTypeHandle(type.Type);
                       using var saveQueryChunks = query.CreateArchetypeChunkArray(Allocator.Temp);

                       using var saveQueryEntKeys = query.ToComponentDataArray<PrefabKeyInfo>(Allocator.Temp);

                       var elementSize = type.ElementSize;
                       
                       
                       //save header info
                       writer.Write(type.StableTypeHash);
                       writer.Write(type.TypeIndex);
                       writer.Write(type.ElementSize);
                       writer.Write(saveQueryChunks.Length);
                       writer.Write(saveQueryEnts.Length);
                       
                       //save Entity info for all Entities associated with this buffer
                       for (int i = 0; i < saveQueryEnts.Length; i++)
                       {
                           writer.Write(saveQueryEnts[i].Index);
                           writer.Write(saveQueryEnts[i].Version);
                           
                           writer.Write(saveQueryEntKeys[i].prefabKey);
                       }

                       // save the buffer data for all of the Entities
                       for (int i = 0; i < saveQueryChunks.Length; i++)
                       {
                           var chunk = saveQueryChunks[i];
                           
                           var bufferAccessor = chunk.GetUntypedBufferAccessor(ref tempComp);
                   
                           for (int ii = 0; ii < bufferAccessor.Length; ii++)
                           {
                               unsafe
                               {
                                   var buffer = bufferAccessor.GetUnsafeReadOnlyPtrAndLength(ii, out var length);
                  
                                   var size = length * elementSize;
                                   var bufferBytes = new byte[size];
                                   Marshal.Copy(new IntPtr(buffer), bufferBytes, 0, size);
                    
                                   writer.Write(size);

                                   writer.Write(bufferBytes);
                                   
                                   // Now stick bufferBytes into whatever structure you are using for the final serialization.
                               }
                           }

                       }
                   }
               }
           }
           
           writer.Close();
       }

       public unsafe void FinalLoader()
       {
           var reader = new BinaryReader(File.Open(SavePath, FileMode.OpenOrCreate));
           
           var blobHashmap = World.GetOrCreateSystem<SetupBlobAssets>().blobHashmap;

           var mapSeed = reader.ReadInt32();
           var totalNumSavedEntities = reader.ReadInt32();
           var numberSavedComponentTypes = reader.ReadInt32();

           var loadedEntitiesRemapper = new NativeParallelHashMap<int2, Entity>(totalNumSavedEntities, Allocator.Temp);


           for (int i = 0; i < numberSavedComponentTypes; i++)
           {
               //read the componentHeader data
               var componentStableHash = reader.ReadUInt64();
               var typeIndex = reader.ReadInt32();
               var savedComponentSize = reader.ReadInt32();
               var numSavedChunks = reader.ReadInt32();
               var numSavedEntities = reader.ReadInt32();
               var elementSize = TypeManager.GetTypeInfo(typeIndex).ElementSize;
               
               var type = TypeManager.GetType(typeIndex);

               for (int ii = 0; ii < numSavedEntities; ii++)
               {
                   var currentEntityIndex = reader.ReadInt32();
                   var currentEntityVersion = reader.ReadInt32();
                   var currentEntityKey = reader.ReadInt32();

                   var tempKey = new int2(currentEntityIndex, currentEntityVersion);

                   if (!loadedEntitiesRemapper.ContainsKey(tempKey))
                   {
                       //instantiate new loaded entity
                       var loadedEntitiy = EntityManager.Instantiate(blobHashmap[currentEntityKey]);
                       loadedEntitiesRemapper.Add(tempKey, loadedEntitiy);
                   }
               }

               var newEntityQueryDesc = new EntityQueryDesc()
               {

                   All = new ComponentType[]
                   {
                       type,
                       typeof(PrefabKeyInfo)
                   }
               };
               //get blank chunk datta from newly spawned Entities
               var newEntityQuery = EntityManager.CreateEntityQuery(newEntityQueryDesc);
               var newEntityChunks = newEntityQuery.CreateArchetypeChunkArray(Allocator.Temp);
               var dynamicSavedTypeHandle = GetDynamicComponentTypeHandle(TypeManager.GetType(typeIndex));
               
               if (TypeManager.GetTypeInfo(typeIndex).Category == TypeManager.TypeCategory.ComponentData)
               {
                   for (int ii = 0; ii < numSavedChunks; ii++)
                   {
                       var chunksLengthinBytes = reader.ReadInt32();
                       var chunkByteData = reader.ReadBytes(chunksLengthinBytes);

                       try
                       {

                           var newEntityChunkData = newEntityChunks[ii].GetDynamicComponentDataArrayReinterpret<byte>(dynamicSavedTypeHandle, savedComponentSize);

                           var chunkByteDataUnsafe = new NativeArray<byte>(chunkByteData.Length, Allocator.Temp);
                           chunkByteDataUnsafe.CopyFrom(chunkByteData);

                           var srcPtr = (byte*)chunkByteDataUnsafe.GetUnsafePtr();
                           var dstPtr = (byte*)newEntityChunkData.GetUnsafePtr();
                       
                           UnsafeUtility.MemCpy(dstPtr, srcPtr, chunkByteDataUnsafe.Length);

                           newEntityChunkData.Dispose();
                           chunkByteDataUnsafe.Dispose();

                       }
                       catch (Exception e)
                       {
                           Debug.Log("Shits fucked");
                           Console.WriteLine(e);
                           throw;
                       }
                   }

               }
               //copy saved buffer data to newly spawned Entities 
               else if (TypeManager.GetTypeInfo(typeIndex).Category == TypeManager.TypeCategory.BufferData)
               {
                   if (type != null && type.GetCustomAttribute(typeof(SaveComponent)) != null)
                   {
                       for (int ii = 0; ii < numSavedChunks; ii++)
                       {
          
                           var bufferAccessor =
                               newEntityChunks[ii].GetUntypedBufferAccessor(ref dynamicSavedTypeHandle);

                           for (int iii = 0; iii < bufferAccessor.Length; iii++)
                           {
                               unsafe
                               {
                                   var chunksLengthinBytes = reader.ReadInt32();
                                   var chunkByteData = reader.ReadBytes(chunksLengthinBytes);

                                   bufferAccessor.ResizeUninitialized(iii, chunkByteData.Length / TypeManager.GetTypeInfo(typeIndex).ElementSize);
                                   var buffer = bufferAccessor.GetUnsafePtrAndLength(iii, out var length);
                             
                                   var size = length * elementSize;
                                   Marshal.Copy(chunkByteData, 0, new IntPtr(buffer), chunkByteData.Length);
                          
                               }
                           }

                       }
                   }
               }
               newEntityChunks.Dispose();
               newEntityQuery.Dispose();
           }

           //Reset IComponentData Entity references
           foreach (var type in TypeManager.AllTypes)
           {
               if (type.Category == TypeManager.TypeCategory.ComponentData)
               {
                   if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                   {
                       var offsets = TypeManager.GetEntityOffsets(type.TypeIndex, out int offsetCount);

                       var resetReferenceQuery = EntityManager.CreateEntityQuery(type.Type);
                       
                       var resetReferenceQueryChunks = resetReferenceQuery.CreateArchetypeChunkArray(Allocator.Temp);

                       var typeHandle = GetDynamicComponentTypeHandle(type.Type);

                       for (int i = 0; i < resetReferenceQueryChunks.Length; i++)
                       {
                           var chunk = resetReferenceQueryChunks[i];
                           var componentBytes = chunk.GetDynamicComponentDataArrayReinterpret<byte>(typeHandle, type.ElementSize);

                           var numComponentsInChunk = componentBytes.Length / type.ElementSize;

                           for (int ii = 0; ii < numComponentsInChunk; ii++)
                           {
                               var componentSlice = componentBytes.Slice(ii * type.ElementSize);
                           
                               var srcPtr = (byte*)componentSlice.GetUnsafePtr();
                               RemapEntityFields(srcPtr, offsets, offsetCount, loadedEntitiesRemapper);
                               
                           }

                           componentBytes.Dispose();
                       }

                       resetReferenceQueryChunks.Dispose();
                       resetReferenceQuery.Dispose();
                   }
               }
           }


           //reset IBufferElementData Entity references
           foreach (var type in TypeManager.AllTypes)
           {
               if (type.Category == TypeManager.TypeCategory.BufferData)
               {
                   if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                   {
                       var offsets = TypeManager.GetEntityOffsets(type.TypeIndex, out int offsetCount);

                       var resetReferenceQuery = EntityManager.CreateEntityQuery(type.Type);
                       
                       var resetReferenceQueryChunks = resetReferenceQuery.CreateArchetypeChunkArray(Allocator.Temp);
                       var typeHandle = GetDynamicComponentTypeHandle(type.Type);

                       for (int i = 0; i < resetReferenceQueryChunks.Length; i++)
                       {
                           var bufferAccessor =
                               resetReferenceQueryChunks[i].GetUntypedBufferAccessor(ref typeHandle);
                           for (int iii = 0; iii < bufferAccessor.Length; iii++)
                           {
                               unsafe
                               {
                                   var buffer = (byte*)bufferAccessor.GetUnsafePtrAndLength(iii, out var length);
                             
                                   var size = length * type.ElementSize;
                                 
                                   RemapEntityFields(buffer, offsets, offsetCount, loadedEntitiesRemapper);
                               }
                           }

                       }
                       resetReferenceQuery.Dispose();
                       resetReferenceQueryChunks.Dispose();
                   }
               }
           }
           loadedEntitiesRemapper.Dispose();
           reader.Close();
       }
       
       
       public static unsafe void RemapEntityFields([ReadOnly] byte* ptr, TypeManager.EntityOffsetInfo* offsets, int offsetCount, NativeParallelHashMap<int2, Entity> remap)
       {
           Debug.Log("remapEntityFields is called");
           for (var i = 0; i < offsetCount; i++)
           {
               var entity = (Entity*)(ptr + offsets[i].Offset);
               
               var tempIndex = entity->Index;
               var tempVersion = entity->Version;
               var tempKey = new int2 { x = tempIndex, y = tempVersion };
               
               *entity = remap.TryGetValue(tempKey, out var newEntity) ? newEntity : Entity.Null;
           }
       }

       public void ReplaceEntities<T>(T inputComponent, Type inputType,
           NativeParallelHashMap<Entity, Entity> oldNewEntityMap) where T : IComponentData
       {
           ReplaceOldEntities(inputType, inputComponent, oldNewEntityMap);
       }

       public void ReplaceOldEntities(Type type, IComponentData component,
           NativeParallelHashMap<Entity, Entity> oldNewEntityMap)
       {
           var properties = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
 
           foreach (var property in properties)
           {
               if (property.FieldType == typeof(Entity))
               {
                   var oldEntity = (Entity) property.GetValue(component);
                   if (oldNewEntityMap.ContainsKey(oldEntity))
                   {
                       property.SetValue(component, oldNewEntityMap[oldEntity]);
                   }
                   else
                   {
                       property.SetValue(component, Entity.Null);
                   }
               }
           }
       }

       public void testWipe()
       {
           var queryDesc = new EntityQueryDesc()
           {
               All = new ComponentType[]
               {
                   typeof(dataComponent),
                   typeof(PrefabKeyInfo)
               }
           };
                        
           using var query = EntityManager.CreateEntityQuery(queryDesc);
           using var chunks = query.CreateArchetypeChunkArray(Allocator.Temp);

           
           //var chunksData = chunks.Reinterpret<byte>();
           var typeI = TypeManager.GetTypeIndex(typeof(dataComponent));
           var typeInfo = TypeManager.GetTypeInfo(typeI);
           var typeLength = typeInfo.ElementSize;

           for (var index = 0; index < chunks.Length; index++)
           {
               var chunk = chunks[index];
               var tempComp = GetDynamicComponentTypeHandle(typeof(dataComponent));

               var componentBytes = chunk.GetDynamicComponentDataArrayReinterpret<byte>(tempComp, typeLength);
               Debug.Log($"size of ComponentBytes: {componentBytes.Length}");

               for (int i = 0; i < componentBytes.Length; i++)
               {
                   var eatShit = componentBytes[i];
                   eatShit = Byte.MinValue;
                   componentBytes[i] = eatShit;
               }

               /*var c = chunksData[index];
               c = 00000000;
               chunksData[index] = c;*/
           }
       }

       
       public void SaveGameBinaryArrayReflection()
    {
        Debug.Log("Saving the game in binaryArray!");

        var writer = new BinaryWriter(File.Open(SavePath, FileMode.OpenOrCreate));
            foreach (var type in TypeManager.AllTypes)
            {                      
                //Debug.Log($"Type {type}   {type.TypeIndex}");

                
                if (type.Category == TypeManager.TypeCategory.ComponentData)
                {
                    
                    if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                    {
                        var queryDesc = new EntityQueryDesc()
                        {
                            All = new ComponentType[]
                            {
                                type.Type,
                                typeof(PrefabKeyInfo)
                            }
                        };
                        
                        using var query = EntityManager.CreateEntityQuery(queryDesc);
                        using var queryEntities = query.ToEntityArray(Allocator.Temp);
                        
                        var tempComp = GetDynamicComponentTypeHandle(type.Type);
                        using var chunks = query.CreateArchetypeChunkArray(Allocator.Temp);

                        using var prefabKeyInfo = query.ToComponentDataArray<PrefabKeyInfo>(Allocator.Temp);
                        
                        var entityHandle = GetEntityTypeHandle();
                        
                        writer.Write(query.CalculateEntityCount());

                        var tempComponentLength = type.ElementSize;
                        var numEntities = query.CalculateEntityCount();
                        
                        
                        writer.Write(type.TypeIndex);
                        writer.Write(tempComponentLength);
                        writer.Write(numEntities);

                        /*var chunksData = chunks.Reinterpret<byte>();

                       writer.Write(chunksData.Length);*/

                       int tempIndex = 0;
                        foreach (var chunk in chunks)
                        {
                            var entities = chunk.GetNativeArray(entityHandle);
                            var componentBytes = chunk.GetDynamicComponentDataArrayReinterpret<byte>(tempComp, type.ElementSize);
                            //var components = chunk.GetChunkComponentData<PrefabKeyInfo>();

                            if (componentBytes.Length == 0) continue;
                            
                            var componentLength = componentBytes.Length / chunk.ChunkEntityCount;


                            for (int i = 0; i < chunk.ChunkEntityCount; i++)
                            {

                                writer.Write(entities[i].Index);
                                writer.Write(entities[i].Version);
                                writer.Write(prefabKeyInfo[tempIndex].prefabKey);
                                writer.Write(componentBytes.ToArray());
                                tempIndex++;
                                
                                //writer.Write(typeof(ChunkEntitiesDescription));
                            }
                            
                            //writer.Write(chunksData.ToArray());
                            
                        }
                    }
                }
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

        /*var saveList = new List<dataComponent>();
        var jsonList = new JsonArray();*/
        
        foreach (var ent in saveEnts)
        {
            var entData = EntityManager.GetComponentData<dataComponent>(ent);
            /*jsonList.Add(entData);
            saveList.Add(entData);*/

            entData.previousIndexNum = new int2(ent.Index, ent.Version);
            entData.previousParentIndexNum = new int2(entData.parentEnt.Index, entData.parentEnt.Version);
            
            
            
            saveDataObj.saveDat.Add(entData);



            /*foreach (var type in TypeManager.AllTypes)
            {                      
                //Debug.Log($"Type {type}   {type.TypeIndex}");

                if (type.Category == TypeManager.TypeCategory.ComponentData)
                {
                    
                    if (type.Type != null && type.Type.GetCustomAttribute(typeof(SaveComponent)) != null)
                    {
                       // var saver = new ComponentSave(this, typeIndex);
                       
                       Debug.Log($"Type {type.Type}  {type.TypeIndex}");
                       
                       var tempComp = GetDynamicComponentTypeHandle(type.Type);

                    }
                }
            }*/
            
            
            //data = data + JsonUtility.ToJson(entData);
            // JsonArray
            //System.IO.File.AppendAllText("D:\\GameDev\\TestingGrounds\\Assets\\SaveData.json", data);

        }
        
        
        //===================================================================
        
        
       
        //===================================================================

        string temp = JsonUtility.ToJson(saveDataObj, true);
        System.IO.File.WriteAllText(SavePath, temp);
    
    }

    public void LoadGameBinary()
    {
        Debug.Log("Loading game in binary...");

        var binaryFormatter = new BinaryFormatter();
        
        var blobHashmap = World.GetOrCreateSystem<SetupBlobAssets>().blobHashmap;

        using (BinaryReader reader = new BinaryReader(File.Open(SavePath, FileMode.OpenOrCreate)))
        {
            //reader.

            //total header
            var numEntitiesTotal = reader.ReadInt32();
            //component header
            var typeIndex = reader.ReadInt32();
            var componentLength = reader.ReadInt32();
            var numberOfComponents = reader.ReadInt32();
            
            var entityRemap = new NativeParallelHashMap<int2, Entity>(numEntitiesTotal, Allocator.Temp);

            //Type componentType;


 //           Debug.Log($"numEntitiesTotal: {numEntitiesTotal}  componentLength: {componentLength}  numberOfComponents: {numberOfComponents}");
            for (int i = 0; i < numberOfComponents; i++)
            {
                var entityID = new int2(reader.ReadInt32(), reader.ReadInt32());
                
                //binaryReader approach
                var entityDataBytes = reader.ReadBytes(componentLength);
                //var acomponentType = TypeManager.GetType(typeIndex);
                var componentType = TypeManager.GetType(typeIndex);
                var managedType = TypeManager.GetTypeInfo(typeIndex);

//                Debug.Log(managedType.Type);

                
               // var componentType = TypeManager.GetType(typeIndex);

              // var componentType2 = typeof();
 //               Debug.Log($"type: {componentType}  index: {typeIndex}");
                var componentHandle = GetDynamicComponentTypeHandle(componentType);
                ComponentType ct = componentType;
                
                var entityData = Deserialize<dataComponent>(entityDataBytes);

              // var entityData = ConvertBytesToStruct(componentType, entityDataBytes);


               //var tempTemp = componentType.GetConstructor();
               Debug.Log($" new object loaded in: {entityData.GetType()}");
               
               
               
               /*
               var size = Marshal.SizeOf(TypeManager.GetType(typeIndex));
               var ptr = Marshal.AllocHGlobal(size);
               Marshal.Copy(entityDataBytes, 0, ptr, size);
               var ouputStruct = Marshal.PtrToStructure(ptr, TypeManager.GetType(typeIndex));
               Marshal.FreeHGlobal(ptr);*/

                //var entityData = entityDataBytes.r
                
                //    binaryFormatter approach
                /*var dataStream = new MemoryStream(entityDataBytes);
                Debug.Log($"dataStreamLength = {dataStream.Length}");
                var entityData = (dataComponent)binaryFormatter.Deserialize(dataStream);*/
                    
                
                
                if (!entityRemap.ContainsKey(entityID))
                {
                    var tempEnt = EntityManager.Instantiate(blobHashmap[(int)entityData.entityType]);
                    entityRemap.Add(entityID, tempEnt);
                    
                    //EntityManager.SetComponentData<object>(EntityManager, tempEnt, entityData);
                    EntityManager.SetComponentData(tempEnt, entityData);
                    EntityManager.SetComponentData(tempEnt, new Translation(){ Value = new float3(entityData.pos.x, 0, entityData.pos.y)});
                    
                    
                }
                else
                {

                    var loadedEnt = entityRemap[entityID];
                    EntityManager.SetComponentData(loadedEnt, entityData);
                    EntityManager.SetComponentData(loadedEnt, new Translation(){ Value = new float3(entityData.pos.x, 0, entityData.pos.y)});

                }
                
            }

            for (int i = 0; i < numEntitiesTotal; i++)
            {
                
            }
            entityRemap.Dispose();
        }
        
        
    }
    
    
     public void LoadGameBinaryArray()
    {
        Debug.Log("Loading game in binary...");
        
        var blobHashmap = World.GetOrCreateSystem<SetupBlobAssets>().blobHashmap;

        using (BinaryReader reader = new BinaryReader(File.Open(SavePath, FileMode.OpenOrCreate)))
        {
            //total header
            var numEntitiesTotal = reader.ReadInt32();
            //component header
            var typeIndex = reader.ReadInt32();
            var componentLength = reader.ReadInt32();
            var numberofEntitiesforCurrentComponent = reader.ReadInt32();
            var lengthOfDataArray = reader.ReadInt32();
            byte[] saveData = new byte[lengthOfDataArray];

            var entityRemap = new NativeParallelHashMap<int2, Entity>(numEntitiesTotal, Allocator.Temp);

            var componentType = TypeManager.GetType(typeIndex);
            
 //           Debug.Log($"numEntitiesTotal: {numEntitiesTotal}  componentLength: {componentLength}  numberOfComponents: {numberOfComponents}");
            for (int i = 0; i < numberofEntitiesforCurrentComponent; i++)
            {
                var entityID = new int2(reader.ReadInt32(), reader.ReadInt32());
                var entityPrefabKey = reader.ReadInt32();
                
                /*//binaryReader approach
                var entityDataBytes = reader.ReadBytes(componentLength);
               // var componentType = TypeManager.GetType(typeIndex);
                
                var entityData = Deserialize<dataComponent>(entityDataBytes);

               Debug.Log($" new object loaded in: {entityData.GetType()}");*/

               if (!entityRemap.ContainsKey(entityID))
                {
                    var tempEnt = EntityManager.Instantiate(blobHashmap[entityPrefabKey]);
                    entityRemap.Add(entityID, tempEnt);
                    
                    //EntityManager.SetComponentData<object>(EntityManager, tempEnt, entityData);
                   // EntityManager.SetComponentData(tempEnt, entityData);
                   // EntityManager.SetComponentData(tempEnt, new Translation(){ Value = new float3(entityData.pos.x, 0, entityData.pos.y)});
                    
                    
                }
                else
                {

                    var loadedEnt = entityRemap[entityID];
                   // EntityManager.SetComponentData(loadedEnt, entityData);
                   // EntityManager.SetComponentData(loadedEnt, new Translation(){ Value = new float3(entityData.pos.x, 0, entityData.pos.y)});

                }


               saveData = reader.ReadBytes(lengthOfDataArray);
            }
            
            var queryDesc = new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    TypeManager.GetType(typeIndex),
                    typeof(PrefabKeyInfo)
                }
            };

            var saveData2 = new NativeArray<byte>(lengthOfDataArray, Allocator.Temp);
            for (var index = 0; index < saveData.Length; index++)
            {
                var VARIABLE = saveData[index];
                saveData2[index] = VARIABLE;
            }

            using var query = EntityManager.CreateEntityQuery(queryDesc);
            var newChunkData =  query.CreateArchetypeChunkArray(Allocator.Temp);
            var tempComp = GetDynamicComponentTypeHandle(componentType);

            for (var index = 0; index < newChunkData.Length; index++)
            {
                unsafe
                {
                    var chunk = newChunkData[index];
                    var componentBytes = chunk.GetDynamicComponentDataArrayReinterpret<byte>(tempComp, componentLength);

                    var dstPtr = (byte*)componentBytes.GetUnsafePtr();
                    var srcPtr = (byte*)saveData2.GetUnsafePtr();
                    UnsafeUtility.MemCpy(dstPtr, srcPtr, componentBytes.Length);
                }
            }

            for (int i = 0; i < numEntitiesTotal; i++)
            {
                
            }
            entityRemap.Dispose();
        }
        
        
    }
    
     
     public void LoadGameBinaryArrayReflection()
    {
        Debug.Log("Loading game in binary...");
        
        var blobHashmap = World.GetOrCreateSystem<SetupBlobAssets>().blobHashmap;

        using (BinaryReader reader = new BinaryReader(File.Open(SavePath, FileMode.OpenOrCreate)))
        {
            //total header
            var numEntitiesTotal = reader.ReadInt32();
            //component header
            var typeIndex = reader.ReadInt32();
            var componentLength = reader.ReadInt32();
            var numberofEntitiesforCurrentComponent = reader.ReadInt32();
            var lengthOfDataArray = reader.ReadInt32();
            
            var entityRemap = new NativeParallelHashMap<int2, Entity>(numEntitiesTotal, Allocator.Temp);
            
 //           Debug.Log($"numEntitiesTotal: {numEntitiesTotal}  componentLength: {componentLength}  numberOfComponents: {numberOfComponents}");
            for (int i = 0; i < numberofEntitiesforCurrentComponent; i++)
            {
                var entityID = new int2(reader.ReadInt32(), reader.ReadInt32());
                var entityPrefabKey = reader.ReadInt32();
                
                //binaryReader approach
                var entityDataBytes = reader.ReadBytes(componentLength);
               // var componentType = TypeManager.GetType(typeIndex);
                
                var entityData = Deserialize<dataComponent>(entityDataBytes);

                var entData = Activator.CreateInstance(TypeManager.GetType(typeIndex));
                

               Debug.Log($" new object loaded in: {entityData.GetType()}");

               if (!entityRemap.ContainsKey(entityID))
                {
                    var tempEnt = EntityManager.Instantiate(blobHashmap[entityPrefabKey]);
                    entityRemap.Add(entityID, tempEnt);
                    
                    //EntityManager.SetComponentData<object>(EntityManager, tempEnt, entityData);
                   // EntityManager.SetComponentData(tempEnt, entityData);
                   // EntityManager.SetComponentData(tempEnt, new Translation(){ Value = new float3(entityData.pos.x, 0, entityData.pos.y)});
                    
                    
                }
                else
                {

                    var loadedEnt = entityRemap[entityID];
                   // EntityManager.SetComponentData(loadedEnt, entityData);
                   // EntityManager.SetComponentData(loadedEnt, new Translation(){ Value = new float3(entityData.pos.x, 0, entityData.pos.y)});

                }
                
            }
            
            var queryDesc = new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    TypeManager.GetType(typeIndex),
                    typeof(PrefabKeyInfo)
                }
            };
                        
            using var query = EntityManager.CreateEntityQuery(queryDesc);
            var newChunkData =  query.CreateArchetypeChunkArray(Allocator.Temp);

            foreach (var chunk in newChunkData)
            {
                //UnsafeUtility.MemCpy();
            }

            for (int i = 0; i < numEntitiesTotal; i++)
            {
                
            }
            entityRemap.Dispose();
        }
        
        
    }
    
    public object ConvertBytesToStruct(Type type, byte[] bytes)
    {
        var genType = typeof(LowLevelHelper).GetMethod("As", BindingFlags.Static | BindingFlags.Public);
        var unsafeAs = genType!.MakeGenericMethod(type);
        var component = unsafeAs!.Invoke(null, new object[]
        {
            bytes,
        });
        return component;
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

    public byte[] SerializeStruct<T>(T inputStruct)
        where T : struct
    {
        var size = Marshal.SizeOf(typeof(T));
        var array = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(inputStruct, ptr, true);
        Marshal.Copy(ptr, array, 0, size);
        Marshal.FreeHGlobal(ptr);
        return array;
    }
    
    public static T Deserialize<T>(byte[] array)
        where T : struct
    {
        var size = Marshal.SizeOf(typeof(T));
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(array, 0, ptr, size);
        var ouputStruct = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return ouputStruct;
    }
    
    /*public static unsafe T Deserialize2<T>(byte[] buffer) where T : unmanaged
    {
        T result = new T();

        fixed (byte* bufferPtr = buffer)
        {
            Buffer.MemoryCopy(bufferPtr, &result, sizeof(T), sizeof(T));
        }

        return result;
    }*/
}

public partial struct SavingJob : IJobEntity
{
    // Adds one to every translation component
    void Execute(ref dataComponent savDat)
    {
        
    }
}