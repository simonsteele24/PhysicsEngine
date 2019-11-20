using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public class PhysicsNativePlugin
{
    [DllImport("VeryEmpty")]
    public static extern void CreatePhysicsWorld();

    [DllImport("VeryEmpty")]
    public static extern void DestroyPhysicsWorld();

    [DllImport("VeryEmpty")]
    public static extern void UpdatePhysicsWorld(float deltaTime);

    [DllImport("VeryEmpty")]
    public static extern int GetCollision();

    [DllImport("VeryEmpty")]
    public static extern void ChangePosition(ref float x, ref float y, ref float z);
}
