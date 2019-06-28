using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RotationFocusBehavior : MonoBehaviour, IConvertGameObjectToEntity
{
    public float RotationSpeed = 1f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new RotationFocus());

        dstManager.AddComponentData(entity, new RotationData
        {
            Value = RotationSpeed
        });
    }    
}
