using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHull2D : MonoBehaviour
{
    public CollisionManager.CollisionHullType collisionType;
    public float radius;
    [HideInInspector] public Vector2 position;
    public float rotation;

    public float halfLength;
    public float halfWidth;

    public Vector2 minCorner;
    public Vector2 maxCorner;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;

       // minCorner = new Vector2(position.x - halfLength, position.y - halfWidth);
        //maxCorner = new Vector2(position.x + halfLength, position.y + halfWidth);

        CollisionManager.manager.InsertToParticleList(this);
    }
}
