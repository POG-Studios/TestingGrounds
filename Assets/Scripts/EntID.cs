
using Unity.Entities;

[GenerateAuthoringComponent]
[SaveComponent]
public struct EntID : IComponentData
{
    public int ID;
}
