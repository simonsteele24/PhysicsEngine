using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager manager;

    public enum CollisionHullType
    {
        Circle,
        AABB,
        OBBB
    }

    public List<CollisionHull2D> particles;

    private void Awake()
    {
        particles = new List<CollisionHull2D>();
        manager = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < particles.Count; x++)
        {
            for (int y = 0; y < particles.Count; y++)
            {
                if (x != y)
                {
                    if (particles[x].collisionType == CollisionHullType.Circle && particles[y].collisionType == CollisionHullType.Circle)
                    {
                        Debug.Log(particles[x].name + " and " + particles[y].name + " are colliding: " + CircleToCircleCollision(particles[x], particles[y]));
                    }
                    if (particles[x].collisionType == CollisionHullType.AABB && particles[y].collisionType == CollisionHullType.AABB)
                    {
                        Debug.Log(particles[x].name + " and " + particles[y].name + " are colliding: " + AABBToAABBCollision(particles[x], particles[y]));
                    }
                    if (particles[x].collisionType == CollisionHullType.OBBB && particles[y].collisionType == CollisionHullType.OBBB)
                    {
                        Debug.Log(particles[x].name + " and " + particles[y].name + " are colliding: " + OBBToOBBCollision(particles[x], particles[y]));
                    }
                }
            }
        }
    }

    public void InsertToParticleList(CollisionHull2D collision)
    {
        particles.Add(collision);
    }

    public static bool CircleToCircleCollision(CollisionHull2D a, CollisionHull2D b)
    {
        float distance = (a.position - b.position).magnitude;
        float radius = a.radius + b.radius;
        return distance <= radius;
    }

    public static bool AABBToAABBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        bool xAxisCheck = a.minCorner.x <= b.maxCorner.x && b.minCorner.x <= a.maxCorner.x;
        bool yAxisCheck = a.minCorner.y <= b.maxCorner.y && b.minCorner.y <= a.maxCorner.y;

        return xAxisCheck && yAxisCheck;
    }

    public static bool OBBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        Vector2 ARHat = new Vector2(Mathf.Cos(a.rotation), Mathf.Sin(a.rotation));
        Vector2 BRHat = new Vector2(Mathf.Cos(b.rotation), Mathf.Sin(b.rotation));
        Vector2 AUHat = new Vector2(-Mathf.Sin(a.rotation), Mathf.Cos(a.rotation));
        Vector2 BUHat = new Vector2(-Mathf.Sin(b.rotation), Mathf.Cos(b.rotation));


        Vector2 newAMin = Vector2.Dot(a.minCorner, ARHat) * ARHat;
        Vector2 newAMax = Vector2.Dot(a.maxCorner, ARHat) * ARHat;
        Vector2 newBMin = Vector2.Dot(b.minCorner, ARHat) * ARHat;
        Vector2 newBMax = Vector2.Dot(b.maxCorner, ARHat) * ARHat;

        bool xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        bool yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        if (!(xAxisCheck && yAxisCheck))
        {
            return false;
        }


        

        newAMin = Vector2.Dot(a.minCorner, BRHat) * BRHat;
        newAMax = Vector2.Dot(a.maxCorner, BRHat) * BRHat;
        newBMin = Vector2.Dot(b.minCorner, BRHat) * BRHat;
        newBMax = Vector2.Dot(b.maxCorner, BRHat) * BRHat;

        xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        if (!(xAxisCheck && yAxisCheck))
        {
            return false;
        }




        newAMin = Vector2.Dot(a.minCorner, AUHat) * AUHat;
        newAMax = Vector2.Dot(a.maxCorner, AUHat) * AUHat;
        newBMin = Vector2.Dot(b.minCorner, AUHat) * AUHat;
        newBMax = Vector2.Dot(b.maxCorner, AUHat) * AUHat;

        xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        if (!(xAxisCheck && yAxisCheck))
        {
            return false;
        }

        Debug.Log("Check");



        newAMin = Vector2.Dot(a.minCorner, BUHat) * BUHat;
        newAMax = Vector2.Dot(a.maxCorner, BUHat) * BUHat;
        newBMin = Vector2.Dot(b.minCorner, BUHat) * BUHat;
        newBMax = Vector2.Dot(b.maxCorner, BUHat) * BUHat;

        xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        if (!(xAxisCheck && yAxisCheck))
        {
            return false;
        }

        Debug.Log("Check");

        return true;
    }
}
