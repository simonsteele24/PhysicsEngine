using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;

[Serializable]
public struct AngularVelocityData : IComponentData
{
    public Matrix4x4 inertiaTensor;
    public Quaternion rotation;
    public Vector3 angularAcceleration;
    public Vector3 angularVelocity;
    public Vector3 torque;
    public Vector3 force;
    public Vector3 acceleration;
    public Vector3 velocity;
    public float invMass;
    public Vector3 position;
}
