using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;

[Serializable]
public class PositionData : IComponentData
{
    public Vector3 position;
}
