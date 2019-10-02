using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager manager;

    // Collision Hull type enum
    public enum CollisionHullType
    {
        Circle,
        AABB,
        OBBB
    }

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
        // Iterate through all particles
        for (int x = 0; x < particles.Count; x++)
        {
            for (int y = 0; y < particles.Count; y++)
            {
                // If the one being checked equal to itself?
                if (x != y)
                {
                    // If no, then check for collisions
                    // Are the colliders both circles?
                    if (particles[x].collisionType == CollisionHullType.Circle && particles[y].collisionType == CollisionHullType.Circle)
                    {
                        // If yes then check if the two are colliding and appropriately highlight them (red = not colliding, green = colliding)
                        if (CircleToCircleCollision(particles[x], particles[y]))
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        }
                        else
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        }
                        
                    }

                    // Are the colliders both AABB?
                    if (particles[x].collisionType == CollisionHullType.AABB && particles[y].collisionType == CollisionHullType.AABB)
                    {
                        // If yes then check if the two are colliding and appropriately highlight them (red = not colliding, green = colliding)
                        if (AABBToAABBCollision(particles[x], particles[y]))
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        }
                        else
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        }
                    }

                    // Are the colliders both OBBBs?
                    if (particles[x].collisionType == CollisionHullType.OBBB && particles[y].collisionType == CollisionHullType.OBBB)
                    {
                        // If yes then check if the two are colliding and appropriately highlight them (red = not colliding, green = colliding)
                        if (OBBToOBBCollision(particles[x], particles[y]))
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        }
                        else
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        }
                    }

                    // Are the colliders AABB and OBBB?
                    if (particles[x].collisionType == CollisionHullType.AABB && particles[y].collisionType == CollisionHullType.OBBB)
                    {
                        // If yes then check if the two are colliding and appropriately highlight them (red = not colliding, green = colliding)
                        if (AABBToOBBCollision(particles[x], particles[y]))
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        }
                        else
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        }
                    }

                    // Are the colliders AABB and Circle?
                    if (particles[x].collisionType == CollisionHullType.AABB && particles[y].collisionType == CollisionHullType.Circle)
                    {
                        // If yes then check if the two are colliding and appropriately highlight them (red = not colliding, green = colliding)
                        if (CircleToABBCollision(particles[x], particles[y]))
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        }
                        else
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        }
                    }

                    // Are the colliders OBBB and Circle?
                    if (particles[x].collisionType == CollisionHullType.OBBB && particles[y].collisionType == CollisionHullType.Circle)
                    {
                        // If yes then check if the two are colliding and appropriately highlight them (red = not colliding, green = colliding)
                        if (CircleToOBBCollision(particles[x], particles[y]))
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                        }
                        else
                        {
                            particles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                            particles[y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
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
        Vector2 ARHat = new Vector2(Mathf.Cos(a.rotation), Mathf.Sin(a.rotation));
        Vector2 AUHat = new Vector2(Mathf.Sin(a.rotation), Mathf.Cos(a.rotation));


        // Project onto A's x axis
        Vector2 newAMin = Vector2.Dot(a.minCorner, ARHat) * ARHat;
        Vector2 newAMax = Vector2.Dot(a.maxCorner, ARHat) * ARHat;
        Vector2 newBMin = Vector2.Dot(b.minCorner, ARHat) * ARHat;
        Vector2 newBMax = Vector2.Dot(b.maxCorner, ARHat) * ARHat;

        // Do axis checks
        bool xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        bool yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Do the checks fail?
        if (!(xAxisCheck && yAxisCheck))
        {
            // If yes, then they are not colliding
            return false;
        }



        // Project onto A's y axis
        newAMin = Vector2.Dot(a.minCorner, AUHat) * AUHat;
        newAMax = Vector2.Dot(a.maxCorner, AUHat) * AUHat;
        newBMin = Vector2.Dot(b.minCorner, AUHat) * AUHat;
        newBMax = Vector2.Dot(b.maxCorner, AUHat) * AUHat;

        // Do axis checks
        xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Do the checks fail?
        if (!(xAxisCheck && yAxisCheck))
        {
            // If yes, then they are not colliding
            return false;
        }

        // If yes, then inform the parents of the complex shape object (if applicable)
        if (a.transform.parent != null)
        {
            a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
        }
        if (b.transform.parent != null)
        {
            b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
        }

        // Return result
        return true;
    }




    // This function calculates Circle to OBB collisions
    public static bool CircleToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Calculate circle min and max corners
        Vector2 circleMax = new Vector2(b.position.x + b.radius, b.position.y + b.radius);
        Vector2 circleMin = new Vector2(b.position.x - b.radius, b.position.y - b.radius);

        // Do axis check
        bool xAxisCheck = a.minCorner.x <= circleMax.x && circleMin.x <= a.maxCorner.x;
        bool yAxisCheck = a.minCorner.y <= circleMax.y && circleMin.y <= a.maxCorner.y;

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
    public static bool CircleToABBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Find the circle max and min corners
        Vector2 circleMax = new Vector2(b.position.x + b.radius, b.position.y + b.radius);
        Vector2 circleMin = new Vector2(b.position.x - b.radius, b.position.y - b.radius);

        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2(Mathf.Cos(a.rotation), Mathf.Sin(a.rotation));
        Vector2 AUHat = new Vector2(Mathf.Sin(a.rotation), Mathf.Cos(a.rotation));

        // Project onto x axis of A
        Vector2 newAMin = Vector2.Dot(a.minCorner, ARHat) * ARHat;
        Vector2 newAMax = Vector2.Dot(a.maxCorner, ARHat) * ARHat;
        Vector2 newBMin = Vector2.Dot(b.minCorner, ARHat) * ARHat;
        Vector2 newBMax = Vector2.Dot(b.maxCorner, ARHat) * ARHat;

        // Do axis checks
        bool xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        bool yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Does the check fail?
        if (!(xAxisCheck && yAxisCheck))
        {
            // If yes, then they are not colliding
            return false;
        }



        // Project onto y axis of A
        newAMin = Vector2.Dot(a.minCorner, AUHat) * AUHat;
        newAMax = Vector2.Dot(a.maxCorner, AUHat) * AUHat;
        newBMin = Vector2.Dot(b.minCorner, AUHat) * AUHat;
        newBMax = Vector2.Dot(b.maxCorner, AUHat) * AUHat;

        // Do axis checks
        xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Does the check fail?
        if (!(xAxisCheck && yAxisCheck))
        {
            // If yes, then they are not colliding
            return false;
        }

        // If yes, then inform the parents of the complex shape object (if applicable)
        if (a.transform.parent != null)
        {
            a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
        }
        if (b.transform.parent != null)
        {
            b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
        }

        // return result
        return true;
    }




    // This function calculates OBB to OBB colisions
    public static bool OBBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for both collision hulls
        Vector2 ARHat = new Vector2(Mathf.Cos(a.rotation), Mathf.Sin(a.rotation));
        Vector2 BRHat = new Vector2(Mathf.Cos(b.rotation), Mathf.Sin(b.rotation));
        Vector2 AUHat = new Vector2(Mathf.Sin(a.rotation), Mathf.Cos(a.rotation));
        Vector2 BUHat = new Vector2(Mathf.Sin(b.rotation), Mathf.Cos(b.rotation));


        // Project onto the x axis of A
        Vector2 newAMin = Vector2.Dot(a.minCorner, ARHat) * ARHat;
        Vector2 newAMax = Vector2.Dot(a.maxCorner, ARHat) * ARHat;
        Vector2 newBMin = Vector2.Dot(b.minCorner, ARHat) * ARHat;
        Vector2 newBMax = Vector2.Dot(b.maxCorner, ARHat) * ARHat;

        // Do axis checks
        bool xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        bool yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Does the check fail?
        if (!(xAxisCheck && yAxisCheck))
        {
            // If yes, then they aren't colliding
            return false;
        }



        // Project onto the x axis of B
        newAMin = Vector2.Dot(a.minCorner, BRHat) * BRHat;
        newAMax = Vector2.Dot(a.maxCorner, BRHat) * BRHat;
        newBMin = Vector2.Dot(b.minCorner, BRHat) * BRHat;
        newBMax = Vector2.Dot(b.maxCorner, BRHat) * BRHat;

        // Do axis checks
        xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Does the check fail?
        if (!(xAxisCheck && yAxisCheck))
        {
            // If yes, then they aren't colliding
            return false;
        }


        // Project onto the y axis of A
        newAMin = Vector2.Dot(a.minCorner, AUHat) * AUHat;
        newAMax = Vector2.Dot(a.maxCorner, AUHat) * AUHat;
        newBMin = Vector2.Dot(b.minCorner, AUHat) * AUHat;
        newBMax = Vector2.Dot(b.maxCorner, AUHat) * AUHat;

        // Do axis checks
        xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Does the check fail?
        if (!(xAxisCheck && yAxisCheck))
        {
            // If yes, then they aren't colliding
            return false;
        }


        // Project onto the y axis of B
        newAMin = Vector2.Dot(a.minCorner, BUHat) * BUHat;
        newAMax = Vector2.Dot(a.maxCorner, BUHat) * BUHat;
        newBMin = Vector2.Dot(b.minCorner, BUHat) * BUHat;
        newBMax = Vector2.Dot(b.maxCorner, BUHat) * BUHat;

        // Do axis checks
        xAxisCheck = newAMin.x <= newBMax.x && newBMin.x <= newAMax.x;
        yAxisCheck = newAMin.y <= newBMax.y && newBMin.y <= newAMax.y;

        // Does the check fail?
        if (!(xAxisCheck && yAxisCheck))
        {
            // If yes, then they aren't colliding
            return false;
        }

        // If yes, then inform the parents of the complex shape object (if applicable)
        if (a.transform.parent != null)
        {
            a.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();
        }
        if (b.transform.parent != null)
        {
            b.GetComponentInParent<ParentCollisionScript>().ReportCollisionToParent();

        }

        // return result
        return true;
    }
}
