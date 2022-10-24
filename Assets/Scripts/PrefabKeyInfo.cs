using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct PrefabKeyInfo : IComponentData
{
    public int prefabKey;
}
