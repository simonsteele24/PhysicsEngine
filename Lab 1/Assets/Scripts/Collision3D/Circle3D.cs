﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle3D : CollisionHull3D
{
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        collisionType = CollisionHullType3D.Circle;

        // Initialize position of Collision hull
        position = transform.position;

        // Add hull to hull list
        CollisionManager3D.manager.InsertToParticleList(this);
    }

    public override float GetDimensions() { return radius; }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        minCorner = new Vector2(position.x - radius, position.y - radius);
        maxCorner = new Vector2(position.x + radius, position.y + radius);
    }
}