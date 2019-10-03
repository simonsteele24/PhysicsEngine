using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB : CollisionHull2D
{
    // Floats
    public float halfLength;
    public float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        collisionType = CollisionHullType2D.OBBB;

        // Initialize position of Collision hull
        position = transform.position;

        // Add hull to hull list
        CollisionManager.manager.InsertToParticleList(this);
    }

    public override float GetRadius() { return 0; }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        rotation = transform.eulerAngles.z;
        minCorner = new Vector2(transform.localPosition.x - halfLength, transform.localPosition.y - halfWidth);
        maxCorner = new Vector2(transform.localPosition.x + halfLength, transform.localPosition.y + halfWidth);
    }
}
