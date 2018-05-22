using System;
using Unity.Entities;

[Serializable]
public struct PlayerInput : IComponentData {
    
}

public class PlayerInputComponent : ComponentDataWrapper<Radius> {}