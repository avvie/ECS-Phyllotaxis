using System;
using Unity.Mathematics;
using Unity.Entities;

[Serializable]
public struct ResetPositionFlag : IComponentData {
    
}

public class ResetPositionFlagComponent : ComponentDataWrapper<RotationFocus> {}