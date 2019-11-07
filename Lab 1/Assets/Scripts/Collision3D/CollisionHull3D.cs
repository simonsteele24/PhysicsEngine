﻿using UnityEngine;

public abstract class CollisionHull3D : MonoBehaviour
{
    // Collision hull types
    [HideInInspector] public CollisionHullType3D collisionType;

    // Vector 2's
    protected bool isAlreadyColliding;
    protected float rotation;
    protected Vector2 minCorner;
    protected Vector2 maxCorner;
    [HideInInspector] protected Vector3 position;

    public Vector2 GetMinimumCorner() { return minCorner; }

    public Vector2 GetMaximumCorner() { return maxCorner; }

    public Vector3 GetPosition() { return position; }

    public void SetPosition(Vector3 newPos) { position = newPos; }

    public float GetRotation() { return rotation; }

    public void ToggleCollidingChecker() { isAlreadyColliding = true; }

    public void ResetCollidingChecker() { isAlreadyColliding = false; }

    public bool GetCollidingChecker() { return isAlreadyColliding; }

    public abstract float GetDimensions();
}