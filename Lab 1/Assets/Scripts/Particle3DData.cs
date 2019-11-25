using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;

[Serializable]
public struct Particle3DData : IComponentData
{
    // Matrices
    public Matrix4x4 inertiaTensor;

    // Quaternions
    public Quaternion rotation;

    // Vector3's
    public Vector3 angularAcceleration;
    public Vector3 angularVelocity;
    public Vector3 torque;
    public Vector3 force;
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 position;

    // Floats
    public float invMass;
}
