using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class AngularVelocitySpeedAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    // The MonoBehaviour data is converted to ComponentData on the entity.
    // We are specifically transforming from a good editor representation of the data (Represented in degrees)
    // To a good runtime representation (Represented in radians)
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Debug.Log("Here");
        var data = new AngularVelocityData { };
        dstManager.AddComponentData(entity, data);
    }
}
