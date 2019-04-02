using System;
using Unity.Entities;

[Serializable]
public struct RotationData : IComponentData
{
    public float Value;
    public int Index;
    public float Radius;
}

[UnityEngine.DisallowMultipleComponent]
public class RotationSpeedComponent : ComponentDataProxy<RotationData>
{
}
