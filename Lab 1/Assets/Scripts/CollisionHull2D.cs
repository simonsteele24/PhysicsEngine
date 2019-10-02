using UnityEngine;

public class CollisionHull2D : MonoBehaviour
{
    // Collision hull types
    public CollisionManager.CollisionHullType collisionType;
    
    // Floats
    public float radius;
    public float rotation;
    public float halfLength;
    public float halfWidth;

    // Vector 2's
    public Vector2 minCorner;
    public Vector2 maxCorner;
    [HideInInspector] public Vector2 position;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize position of Collision hull
        position = transform.position;

        // Add hull to hull list
        CollisionManager.manager.InsertToParticleList(this);
    }
}
