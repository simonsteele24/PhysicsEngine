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
    public class CollisionInfo
    {
        public struct Contact
        {
            Vector2 point;
            Vector2 normal;
            float restitution;
        }
        public CollisionHull2D a;
        public CollisionHull2D b;
        public Contact[] contacts = new Contact[4];

        public Vector2 closingVelocity;
        public Vector2 penetration;


        bool status;

    }


    public static CollisionManager manager;

    private Dictionary<CollisionPairKey, Func<CollisionHull2D, CollisionHull2D, bool>> _collisionTypeCollisionTestFunctions = new Dictionary<CollisionPairKey, Func<CollisionHull2D, CollisionHull2D, bool>>(new CollisionPairKey.EqualityComparitor());

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

                    CollisionPairKey key = new CollisionPairKey(particles[x].collisionType, particles[y].collisionType);
                    
                    bool collisionCheck = _collisionTypeCollisionTestFunctions[key](particles[x], particles[y]);

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




    // Inserts a particle to the particle list
    public void InsertToParticleList(CollisionHull2D collision)
    {
        particles.Add(collision);
    }




    // This function computes circle to circle collisions
    public static bool CircleToCircleCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Calculate the distance between both colliders
        Vector2 distance = a.GetPosition() - b.GetPosition();

        bool axisCheck = Vector2.Dot(distance, distance) <= (a.GetRadius() + b.GetRadius()) * (a.GetRadius() + b.GetRadius());

        // Are the Radii less than or equal to the distance between both circles?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // Return result
        return axisCheck;
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
        Vector2 ARHat = new Vector2((Mathf.Cos(b.GetRotation())), Mathf.Abs(-Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2((Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

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

        Vector2 closestPointToCircle = new Vector2(Math.Max(b.GetMinimumCorner().x, Math.Min(a.GetPosition().x, b.GetMaximumCorner().x)), Math.Max(b.GetMinimumCorner().y, Math.Min(a.GetPosition().y, b.GetMaximumCorner().y)));

        Vector2 distance = a.GetPosition() - closestPointToCircle;

        bool axisCheck = Vector2.Dot(distance, distance) <= a.GetRadius() * a.GetRadius();

        // Does the check pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // Return result
        return axisCheck;
    }




    // This function calculate Circle to ABB collisions
    public static bool CircleToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2((Mathf.Cos(b.GetRotation())), Mathf.Abs(-Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2((Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

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
        Vector2 ARHat = new Vector2(Mathf.Abs(Mathf.Cos(a.GetRotation())), Mathf.Abs(-Mathf.Sin(a.GetRotation())));
        Vector2 BRHat = new Vector2(Mathf.Abs(Mathf.Cos(b.GetRotation())), Mathf.Abs(-Mathf.Sin(b.GetRotation())));
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
        List<Vector2> shapeAPoints = new List<Vector2>();
        shapeAPoints.Add(new Vector2(shapeA.GetDimensions().x + shapeA.GetPosition().x, shapeA.GetDimensions().y + shapeA.GetPosition().y));
        shapeAPoints.Add(new Vector2(shapeA.GetDimensions().x - shapeA.GetPosition().x, shapeA.GetDimensions().y - shapeA.GetPosition().y));
        shapeAPoints.Add(new Vector2(shapeA.GetDimensions().x - shapeA.GetPosition().x, shapeA.GetDimensions().y + shapeA.GetPosition().y));
        shapeAPoints.Add(new Vector2(shapeA.GetDimensions().x + shapeA.GetPosition().x, shapeA.GetDimensions().y - shapeA.GetPosition().y));

        List<Vector2> shapeBPoints = new List<Vector2>();
        shapeBPoints.Add(new Vector2(shapeB.GetDimensions().x + shapeB.GetPosition().x, shapeB.GetDimensions().y + shapeB.GetPosition().y));
        shapeBPoints.Add(new Vector2(shapeB.GetDimensions().x - shapeB.GetPosition().x, shapeB.GetDimensions().y - shapeB.GetPosition().y));
        shapeBPoints.Add(new Vector2(shapeB.GetDimensions().x - shapeB.GetPosition().x, shapeB.GetDimensions().y + shapeB.GetPosition().y));
        shapeBPoints.Add(new Vector2(shapeB.GetDimensions().x + shapeB.GetPosition().x, shapeB.GetDimensions().y - shapeB.GetPosition().y));

        Vector2 shapeAMin = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 shapeAMax = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
        Vector2 shapeBMin = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 shapeBMax = new Vector2(-Mathf.Infinity, -Mathf.Infinity);

        for (int i = 0; i < shapeAPoints.Count; i++)
        {
            Debug.Log(Quaternion.AngleAxis(shapeA.GetRotation(), Vector3.forward));
            Debug.Log(Quaternion.AngleAxis(shapeB.GetRotation(), Vector3.forward));

            shapeAPoints[i] = Quaternion.AngleAxis(shapeA.GetRotation(), Vector3.forward) * shapeAPoints[i];
            shapeAPoints[i] = Vector2.Dot(shapeAPoints[i], rotationAxis) * rotationAxis;
            shapeBPoints[i] = Quaternion.AngleAxis(shapeB.GetRotation(), Vector3.forward) * shapeBPoints[i];
            shapeBPoints[i] = Vector2.Dot(shapeBPoints[i], rotationAxis) * rotationAxis;

        }

        for (int i = 0; i < shapeAPoints.Count; i++)
        {
            if (shapeAPoints[i].x <= shapeAMin.x && shapeAPoints[i].y <= shapeAMin.y)
            {
                shapeAMin = shapeAPoints[i];
            }
            if (shapeAPoints[i].x >= shapeAMax.x && shapeAPoints[i].y >= shapeAMax.y)
            {
                shapeAMax = shapeAPoints[i];
            }
            if (shapeBPoints[i].x <= shapeBMin.x && shapeBPoints[i].y <= shapeBMin.y)
            {
                shapeBMin = shapeBPoints[i];
            }
            if (shapeBPoints[i].x >= shapeBMax.x && shapeBPoints[i].y >= shapeBMax.y)
            {
                shapeBMax = shapeBPoints[i];
            }
        }

        // Do axis checks
        bool xAxisCheck = shapeAMin.x <= shapeBMax.x && shapeBMin.x <= shapeAMax.x;
        bool yAxisCheck = shapeAMin.y <= shapeBMax.y && shapeBMin.y <= shapeAMax.y;

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
