using System;
using System.Collections.Generic;
using UnityEngine;

// Collision Hull type enum
public enum CollisionHullType2D
{
    Circle,
    AABB,
    OBBB
}

public class CollisionManager : MonoBehaviour
{
    private Dictionary<CollisionPairKey, Func<CollisionHull2D, CollisionHull2D, bool>> _collisionTypeCollisionTestFunctions = new Dictionary<CollisionPairKey, Func<CollisionHull2D, CollisionHull2D, bool>>();
    public static CollisionManager manager;

    // Lists
    public List<CollisionHull2D> particles;





    // Set all the initial values
    private void Awake()
    {
        particles = new List<CollisionHull2D>();
        manager = this;

        _collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.Circle, CollisionHullType2D.Circle), CircleToCircleCollision);
        _collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.AABB, CollisionHullType2D.AABB), AABBToAABBCollision);
        _collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.OBBB, CollisionHullType2D.OBBB), OBBToOBBCollision);
        _collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.Circle, CollisionHullType2D.OBBB), CircleToOBBCollision);
        _collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.Circle, CollisionHullType2D.AABB), CircleToABBCollision);
        _collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.AABB, CollisionHullType2D.OBBB), AABBToOBBCollision);
    }





    // Update is called once per frame
    void Update()
    {
        // Iterate through all particles
        for (int x = 0; x < particles.Count; x++)
        {
            for (int y = 0; y < particles.Count; y++)
            {
                // If the one being checked equal to itself?
                if (x != y)
                {
                    /*CollisionPairKey key = new CollisionPairKey(particles[x].collisionType, particles[y].collisionType);

                    if (_collisionTypeCollisionTestFunctions.ContainsKey(key))
                    {
                        if (_collisionTypeCollisionTestFunctions[key](particles[x], particles[y]))
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        }
                        else
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        }
                    }*/

                    // If no, then check for collisions
                    // Are the colliders both circles?
                    if (particles[x].collisionType == CollisionHullType2D.Circle && particles[y].collisionType == CollisionHullType2D.Circle)
                    {
                        bool collisionCheck = CircleToCircleCollision(particles[x], particles[y]);
                        particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                    }

                    // Are the colliders both AABB?
                    if (particles[x].collisionType == CollisionHullType2D.AABB && particles[y].collisionType == CollisionHullType2D.AABB)
                    {
                        bool collisionCheck = AABBToAABBCollision(particles[x], particles[y]);
                        particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                    }

                    // Are the colliders both OBBBs?
                    if (particles[x].collisionType == CollisionHullType2D.OBBB && particles[y].collisionType == CollisionHullType2D.OBBB)
                    {
                        bool collisionCheck = OBBToOBBCollision(particles[x], particles[y]);
                        particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                    }

                    // Are the colliders AABB and OBBB?
                    if (particles[x].collisionType == CollisionHullType2D.AABB && particles[y].collisionType == CollisionHullType2D.OBBB)
                    {
                        bool collisionCheck = AABBToOBBCollision(particles[x], particles[y]);
                        particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                    }

                    // Are the colliders AABB and Circle?
                    if (particles[y].collisionType == CollisionHullType2D.AABB && particles[x].collisionType == CollisionHullType2D.Circle)
                    {
                        bool collisionCheck = CircleToABBCollision(particles[x], particles[y]);
                        particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                    }

                    // Are the colliders OBBB and Circle?
                    if (particles[y].collisionType == CollisionHullType2D.OBBB && particles[x].collisionType == CollisionHullType2D.Circle)
                    {
                        bool collisionCheck = CircleToOBBCollision(particles[x], particles[y]);
                        particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                    }
                }
            }
        }
    }




    // Inserts a particle to the particle list
    public void InsertToParticleList(CollisionHull2D collision)
    {
        particles.Add(collision);
    }




    // This function computes circle to circle collisions
    public static bool CircleToCircleCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Calculate the distance between both colliders
        float distance = (a.position - b.position).magnitude;

        // Combine the sums of both radii
        float radius = a.radius + b.radius;

        // Are the Radii less than or equal to the distance between both circles?
        if (distance <= radius)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            if (a.transform.parent != null)
            {
                a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
            if (b.transform.parent != null)
            {
                b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
        }

        // Return result
        return distance <= radius;
    }




    // This function computes AABB to AABB collisions
    public static bool AABBToAABBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Do an axis check on both the x and y axes
        bool xAxisCheck = a.minCorner.x <= b.maxCorner.x && b.minCorner.x <= a.maxCorner.x;
        bool yAxisCheck = a.minCorner.y <= b.maxCorner.y && b.minCorner.y <= a.maxCorner.y;

        // Do the two checks pass?
        if (xAxisCheck && yAxisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            if (a.transform.parent != null)
            {
                a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
            if (b.transform.parent != null)
            {
                b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
        }

        // Return the result
        return xAxisCheck && yAxisCheck;
    }




    // This function computes AABB to OBBB collisions
    public static bool AABBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2(Mathf.Cos(b.rotation), Mathf.Sin(b.rotation));
        Vector2 AUHat = new Vector2(Mathf.Sin(b.rotation), Mathf.Cos(b.rotation));

        bool axisCheck = CheckOBBAxis(a, b, AUHat) && CheckOBBAxis(a, b, ARHat);

        // Do all checks pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            if (a.transform.parent != null)
            {
                a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
            if (b.transform.parent != null)
            {
                b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
        }

        // Return result
        return axisCheck;
    }




    // This function calculates Circle to OBB collisions
    public static bool CircleToABBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Calculate circle min and max corners
        Vector2 circleMax = new Vector2(a.position.x + a.radius, b.position.y + a.radius);
        Vector2 circleMin = new Vector2(a.position.x - a.radius, b.position.y - a.radius);

        // Do axis check
        bool xAxisCheck = b.minCorner.x <= circleMax.x && circleMin.x <= b.maxCorner.x;
        bool yAxisCheck = b.minCorner.y <= circleMax.y && circleMin.y <= b.maxCorner.y;

        // Does the check pass?
        if (xAxisCheck && yAxisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            if (a.transform.parent != null)
            {
                a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
            if (b.transform.parent != null)
            {
                b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
        }

        // Return result
        return xAxisCheck && yAxisCheck;
    }




    // This function calculate Circle to ABB collisions
    public static bool CircleToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Find the circle max and min corners
        a.minCorner = new Vector2(a.position.x - a.radius, a.position.y - a.radius);
        a.maxCorner = new Vector2(a.position.x + a.radius, a.position.y + a.radius);

        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2(Mathf.Cos(b.rotation), Mathf.Sin(b.rotation));
        Vector2 AUHat = new Vector2(Mathf.Sin(b.rotation), Mathf.Cos(b.rotation));

        bool axisCheck = CheckOBBAxis(a, b, ARHat) && CheckOBBAxis(a, b, AUHat);

        // Do all checks pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            if (a.transform.parent != null)
            {
                a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
            if (b.transform.parent != null)
            {
                b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
        }

        // return result
        return axisCheck;
    }




    // This function calculates OBB to OBB colisions
    public static bool OBBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for both collision hulls
        Vector2 ARHat = new Vector2(Mathf.Cos(a.rotation), Mathf.Sin(a.rotation));
        Vector2 BRHat = new Vector2(Mathf.Cos(b.rotation), Mathf.Sin(b.rotation));
        Vector2 AUHat = new Vector2(Mathf.Sin(a.rotation), Mathf.Cos(a.rotation));
        Vector2 BUHat = new Vector2(Mathf.Sin(b.rotation), Mathf.Cos(b.rotation));

        bool axisChecks = CheckOBBAxis(a, b, ARHat) && CheckOBBAxis(a, b, AUHat) && CheckOBBAxis(a, b, BRHat) && CheckOBBAxis(a, b, BUHat);


        // Do the axis checks pass?
        if (axisChecks)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            if (a.transform.parent != null)
            {
                a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
            }
            if (b.transform.parent != null)
            {
                b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();

            }
        }
        
        // return result
        return true;
    }




    // This function checks for a collision between two objects by projecting onto a specific axis
    public static bool CheckOBBAxis(CollisionHull2D shapeA, CollisionHull2D shapeB, Vector2 rotationAxis)
    {
        // Project axis
        Vector2 newAMin = Vector2.Dot(shapeA.minCorner, rotationAxis) * rotationAxis;
        Vector2 newAMax = Vector2.Dot(shapeA.maxCorner, rotationAxis) * rotationAxis;
        Vector2 newBMin = Vector2.Dot(shapeA.minCorner, rotationAxis) * rotationAxis;
        Vector2 newBMax = Vector2.Dot(shapeB.maxCorner, rotationAxis) * rotationAxis;

        // Do axis checks
        bool xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        bool yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Return result
        return xAxisCheck && yAxisCheck;
    }
}
