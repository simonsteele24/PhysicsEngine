using System;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using UnityEngine;

public class Particle3DAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    // Floats
    public float mass;

    // Vector3's
    public Vector3 force;
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 angularVelocity;
    public Vector3 angularAcceleration;

    // Shapes
    public ThirdDimensionalShapeType shape;

    // Booleans
    public bool isHollow;

    // This function gets called whenever the gameobject gets converted to an entity
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Calculate all initial data values
        var data = new Particle3DData { };
        data.invMass = 1.0f / mass;
        data.position = transform.position;
        data.force = force;
        data.acceleration = acceleration;
        data.inertiaTensor = InertiaTensor.GetInertiaTensor(GetComponent<Particle3D>(), shape, isHollow);
        data.rotation = transform.rotation;
        data.angularVelocity = angularVelocity;
        data.angularAcceleration = angularAcceleration;

        // Add component to manager
        dstManager.AddComponentData(entity, data);
    }
}
