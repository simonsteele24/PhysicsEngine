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

    [DllImport("VeryEmpty")]
    public static extern int AddParticle(float mass, float x, float y, float z);

    [DllImport("VeryEmpty")]
    public static extern void AddForce(float forceX, float forceY, float forceZ, int element);

    [DllImport("VeryEmpty")]
    public static extern void UpdateParticle(ref float x, ref float y, ref float z, float dt, int element);

    public static bool hasBeenEnabled = false;
}
