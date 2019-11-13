using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB3D : CollisionHull3D
{
    public float halfLength;
    public float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        collisionType = CollisionHullType3D.AABB;

        

        // Initialize position of Collision hull
        position = transform.position;

        minCorner = new Vector3(position.x - halfLength, position.y - halfWidth, position.z - halfWidth);
        maxCorner = new Vector3(position.x + halfLength, position.y + halfWidth, position.z + halfWidth);

        // Add hull to hull list
        CollisionManager3D.manager.InsertToParticleList(this);
    }

    public override float GetDimensions() { return halfLength; }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        minCorner = new Vector3(position.x - halfLength, position.y - halfWidth, position.z - halfWidth);
        maxCorner = new Vector3(position.x + halfLength, position.y + halfWidth, position.z + halfWidth);
    }
}
