﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB3D : CollisionHull3D
{
    // Floats
    public float halfLength;
    public float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        collisionType = CollisionHullType3D.OBBB;

        // Initialize position of Collision hull
        position = transform.position;

        // Add hull to hull list
        CollisionManager3D.manager.InsertToParticleList(this);
    }


    public override float GetDimensions() { return halfLength; }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        rotation = transform.eulerAngles.z;

        Vector3 originalMin = new Vector3(transform.position.x - halfLength, transform.position.y - halfWidth,0);
        Vector3 originalMax = new Vector3(transform.position.x + halfLength, transform.position.y + halfWidth,0);

        minCorner = Quaternion.Euler(0,0,rotation) * (originalMin - transform.position) + transform.position;
        maxCorner = Quaternion.Euler(0,0,rotation) * (originalMax - transform.position) + transform.position;
    }
}