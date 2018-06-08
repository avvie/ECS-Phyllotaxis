using System;
using Unity.Mathematics;
using Unity.Entities;

[Serializable]
public struct ResetPosition : IComponentData {
    
}

public class ResetPositionComponent : ComponentDataWrapper<RotationFocus> {}