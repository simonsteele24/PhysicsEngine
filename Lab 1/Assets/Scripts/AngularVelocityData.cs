using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;

[Serializable]
public struct AngularVelocityData : IComponentData
{
    public Vector3 force;
    public Vector3 acceleration;
    public Vector3 velocity;
    public float invMass;
    public Vector3 position;
}
