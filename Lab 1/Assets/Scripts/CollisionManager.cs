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
    public static CollisionManager manager;

    // Lists
    public List<CollisionHull2D> particles;





    // Set all the initial values
    private void Awake()
    {
        particles = new List<CollisionHull2D>();
        manager = this;
    }





    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].ResetCollidingChecker();
        }

        // Iterate through all particles
        for (int x = 0; x < particles.Count; x++)
        {
            for (int y = 0; y < particles.Count; y++)
            {
                // If the one being checked equal to itself?
                if (x != y && (particles[x].transform.parent != particles[y].transform.parent || particles[x].transform.parent == null))
                {
                    // If no, then check for collisions
                    // Are the colliders both circles?
                    if (particles[x].collisionType == CollisionHullType2D.Circle && particles[y].collisionType == CollisionHullType2D.Circle)
                    {
                        bool collisionCheck = CircleToCircleCollision(particles[x], particles[y]);

                        if (!particles[x].GetCollidingChecker())
                            particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        if (!particles[y].GetCollidingChecker())
                            particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);

                        if (collisionCheck)
                        {
                            particles[x].ToggleCollidingChecker();
                            particles[y].ToggleCollidingChecker();
                        }
                    }

                    // Are the colliders both AABB?
                    if (particles[x].collisionType == CollisionHullType2D.AABB && particles[y].collisionType == CollisionHullType2D.AABB)
                    {
                        bool collisionCheck = AABBToAABBCollision(particles[x], particles[y]);

                        if (!particles[x].GetCollidingChecker())
                            particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        if (!particles[y].GetCollidingChecker())
                            particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);

                        if (collisionCheck)
                        {
                            particles[x].ToggleCollidingChecker();
                            particles[y].ToggleCollidingChecker();
                        }
                    }

                    // Are the colliders both OBBBs?
                    if (particles[x].collisionType == CollisionHullType2D.OBBB && particles[y].collisionType == CollisionHullType2D.OBBB)
                    {
                        bool collisionCheck = OBBToOBBCollision(particles[x], particles[y]);

                        if (!particles[x].GetCollidingChecker())
                            particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        if (!particles[y].GetCollidingChecker())
                            particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);

                        if (collisionCheck)
                        {
                            particles[x].ToggleCollidingChecker();
                            particles[y].ToggleCollidingChecker();
                        }
                    }

                    // Are the colliders AABB and OBBB?
                    if (particles[x].collisionType == CollisionHullType2D.AABB && particles[y].collisionType == CollisionHullType2D.OBBB)
                    {
                        bool collisionCheck = AABBToOBBCollision(particles[x], particles[y]);

                        if (!particles[x].GetCollidingChecker())
                            particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        if (!particles[y].GetCollidingChecker())
                            particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);

                        if (collisionCheck)
                        {
                            particles[x].ToggleCollidingChecker();
                            particles[y].ToggleCollidingChecker();
                        }
                    }

                    // Are the colliders AABB and Circle?
                    if (particles[y].collisionType == CollisionHullType2D.AABB && particles[x].collisionType == CollisionHullType2D.Circle)
                    {
                        bool collisionCheck = CircleToABBCollision(particles[x], particles[y]);

                        if (!particles[x].GetCollidingChecker())
                            particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        if (!particles[y].GetCollidingChecker())
                            particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);

                        if (collisionCheck)
                        {
                            particles[x].ToggleCollidingChecker();
                            particles[y].ToggleCollidingChecker();
                        }
                    }

                    // Are the colliders OBBB and Circle?
                    if (particles[y].collisionType == CollisionHullType2D.OBBB && particles[x].collisionType == CollisionHullType2D.Circle)
                    {
                        bool collisionCheck = CircleToOBBCollision(particles[x], particles[y]);

                        if (!particles[x].GetCollidingChecker())
                            particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);
                        if (!particles[y].GetCollidingChecker())
                            particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisionCheck), Convert.ToInt32(collisionCheck), 0);

                        if (collisionCheck)
                        {
                            particles[x].ToggleCollidingChecker();
                            particles[y].ToggleCollidingChecker();
                        }
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
        float distance = (a.GetPosition() - b.GetPosition()).magnitude;

        // Combine the sums of both radii
        float radius = a.GetRadius() + b.GetRadius();

        // Are the Radii less than or equal to the distance between both circles?
        if (distance <= radius)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // Return result
        return distance <= radius;
    }




    // This function computes AABB to AABB collisions
    public static bool AABBToAABBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Do an axis check on both the x and y axes
        bool xAxisCheck = a.GetMaximumCorner().x <= b.GetMinimumCorner().x && b.GetMinimumCorner().x <= a.GetMaximumCorner().x;
        bool yAxisCheck = a.GetMinimumCorner().y <= b.GetMaximumCorner().y && b.GetMinimumCorner().y <= a.GetMaximumCorner().y;

        // Do the two checks pass?
        if (xAxisCheck && yAxisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // Return the result
        return xAxisCheck && yAxisCheck;
    }




    // This function computes AABB to OBBB collisions
    public static bool AABBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2(Mathf.Abs(Mathf.Cos(b.GetRotation())), Mathf.Abs(Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2(Mathf.Abs(Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

        bool axisCheck = CheckOBBAxis(a, b, AUHat) && CheckOBBAxis(a, b, ARHat);

        // Do all checks pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // Return result
        return axisCheck;
    }




    // This function calculates Circle to OBB collisions
    public static bool CircleToABBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Calculate circle min and max corners
        Vector2 circleMax = new Vector2(a.GetPosition().x + a.GetRadius(), a.GetPosition().y + a.GetRadius());
        Vector2 circleMin = new Vector2(a.GetPosition().x - a.GetRadius(), a.GetPosition().y - a.GetRadius());

        // Do axis check
        bool xAxisCheck = b.GetMinimumCorner().x <= circleMax.x && circleMin.x <= b.GetMaximumCorner().x;
        bool yAxisCheck = b.GetMinimumCorner().y <= circleMax.y && circleMin.y <= b.GetMaximumCorner().y;

        // Does the check pass?
        if (xAxisCheck && yAxisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // Return result
        return xAxisCheck && yAxisCheck;
    }




    // This function calculate Circle to ABB collisions
    public static bool CircleToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2(Mathf.Abs(Mathf.Cos(b.GetRotation())), Mathf.Abs(Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2(Mathf.Abs(Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

        bool axisCheck = CheckOBBAxis(a, b, ARHat) && CheckOBBAxis(a, b, AUHat);

        // Do all checks pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // return result
        return axisCheck;
    }




    // This function calculates OBB to OBB colisions
    public static bool OBBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for both collision hulls
        Vector2 ARHat = new Vector2(Mathf.Abs(Mathf.Cos(a.GetRotation())), Mathf.Abs(Mathf.Sin(a.GetRotation())));
        Vector2 BRHat = new Vector2(Mathf.Abs(Mathf.Cos(b.GetRotation())), Mathf.Abs(Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2(Mathf.Abs(Mathf.Sin(a.GetRotation())), Mathf.Abs(Mathf.Cos(a.GetRotation())));
        Vector2 BUHat = new Vector2(Mathf.Abs(Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

        bool axisChecks = CheckOBBAxis(a, b, ARHat) && CheckOBBAxis(a, b, AUHat) && CheckOBBAxis(a, b, BRHat) && CheckOBBAxis(a, b, BUHat);


        // Do the axis checks pass?
        if (axisChecks)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }
        
        // return result
        return axisChecks;
    }




    // This function checks for a collision between two objects by projecting onto a specific axis
    public static bool CheckOBBAxis(CollisionHull2D shapeA, CollisionHull2D shapeB, Vector2 rotationAxis)
    {
        // Project axis
        Vector2 newAMin = Vector2.Dot(shapeA.GetMinimumCorner(), rotationAxis) * rotationAxis;
        Vector2 newAMax = Vector2.Dot(shapeA.GetMaximumCorner(), rotationAxis) * rotationAxis;
        Vector2 newBMin = Vector2.Dot(shapeB.GetMinimumCorner(), rotationAxis) * rotationAxis;
        Vector2 newBMax = Vector2.Dot(shapeB.GetMaximumCorner(), rotationAxis) * rotationAxis;

        // Do axis checks
        bool xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        bool yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Return result
        return xAxisCheck && yAxisCheck;
    }



    // This function reports two sets of collision hulls to their respective parents (if possible)
    public static void ReportCollisionToParent(CollisionHull2D shapeA, CollisionHull2D shapeB)
    {
        // If yes, then inform the parents of the complex shape object (if applicable)
        if (shapeA.transform.parent != null)
        {
            shapeA.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
        }
        if (shapeB.transform.parent != null)
        {
            shapeB.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();

        }
    }
}
