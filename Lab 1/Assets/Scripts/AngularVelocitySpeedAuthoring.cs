using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class AngularVelocitySpeedAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Vector3 startingForce;

    // The MonoBehaviour data is converted to ComponentData on the entity.
    // We are specifically transforming from a good editor representation of the data (Represented in degrees)
    // To a good runtime representation (Represented in radians)
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Debug.Log("Here");
        var data = new AngularVelocityData { force = startingForce };
        dstManager.AddComponentData(entity, data);
        GetComponentInParent<Particle3D>().velocityEntity = entity;
    }
}
