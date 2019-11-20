using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;

[Serializable]
public struct AngularVelocityData : IComponentData
{
    public Vector3 angularVelocity;
    public Vector3 angularAcceleration;
    public Vector3 torque;
    public Matrix4x4 inertiaTensor;
    public Matrix4x4 inverseInertiaTensor;
}
