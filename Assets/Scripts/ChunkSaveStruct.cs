using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ChunkSaveStruct
{
    public int componentByteLength;
    public Entity[] entityArray;
    public byte[] componentData;

    public ChunkSaveStruct(int inputComponentLength,int inputEntityArrayLength, int inputComponentDataArrayLength) : this()
    {
        this.entityArray = new Entity[inputEntityArrayLength];
        this.componentData = new byte[inputComponentDataArrayLength];
        this.componentByteLength = inputComponentLength;
        
    }
}
