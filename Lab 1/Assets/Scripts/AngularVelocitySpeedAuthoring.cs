using System;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using UnityEngine;

public class AngularVelocitySpeedAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float mass;
    public Vector3 force;
    public Vector3 acceleration;
    public Vector3 velocity;

    // The MonoBehaviour data is converted to ComponentData on the entity.
    // We are specifically transforming from a good editor representation of the data (Represented in degrees)
    // To a good runtime representation (Represented in radians)
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new AngularVelocityData { };
        data.invMass = 1.0f / mass;
        data.position = transform.position;
        data.force = force;
        data.acceleration = acceleration;
        dstManager.AddComponentData(entity, data);
    }
}
