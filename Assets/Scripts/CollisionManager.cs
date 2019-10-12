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
        public Vector2 normal;
        public float penetration;
        public CollisionHull2D a;
        public CollisionHull2D b;

        public float separatingVelocity;

        public bool status;


        public CollisionInfo(bool _status, CollisionHull2D _a, CollisionHull2D _b, float _separatingVelocity, List<float> penetrationList)
        {
            status = _status;
            a = _a;
            b = _b;
            separatingVelocity = _separatingVelocity;
            normal = (_b.GetPosition() - _a.GetPosition()).normalized;
            penetration = penetrationList[0];
        }

    }


    public static CollisionManager manager;

    private Dictionary<CollisionPairKey, Func<CollisionHull2D, CollisionHull2D, CollisionInfo>> _collisionTypeCollisionTestFunctions = new Dictionary<CollisionPairKey, Func<CollisionHull2D, CollisionHull2D, CollisionInfo>>(new CollisionPairKey.EqualityComparitor());

    public static float RESTING_CONTACT_VALUE = 0.1f;

    // Lists
    public List<CollisionHull2D> particles;
    public List<CollisionInfo> collisions;




    // Set all the initial values
    private void Awake()
    {
        particles = new List<CollisionHull2D>();
        collisions = new List<CollisionInfo>();
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
        collisions.Clear();
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


                    bool isDuplicate = false;
                    for (int i = 0; i < collisions.Count; i++)
                    {
                        if ((collisions[i].a == particles[y] && collisions[i].b == particles[x]) || (collisions[i].a == particles[x] && collisions[i].b == particles[y]))
                        {
                            isDuplicate = true;
                        }
                    }

                    if (!isDuplicate)
                    {
                        collisions.Add(_collisionTypeCollisionTestFunctions[key](particles[x], particles[y]));
                    }


                    if (!particles[x].GetCollidingChecker())
                        particles[x].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisions[collisions.Count - 1].status), Convert.ToInt32(collisions[collisions.Count - 1].status), 0);
                    if (!particles[y].GetCollidingChecker())
                        particles[y].GetComponent<Renderer>().material.color = new Color(Convert.ToInt32(!collisions[collisions.Count - 1].status), Convert.ToInt32(collisions[collisions.Count - 1].status), 0);

                    if (collisions[collisions.Count - 1].status)
                    {
                        particles[x].ToggleCollidingChecker();
                        particles[y].ToggleCollidingChecker();
                    }

                }
            }
        }
        CollisionResolution.ResolveCollisions(collisions,Time.deltaTime);
    }





    // Inserts a particle to the particle list
    public void InsertToParticleList(CollisionHull2D collision)
    {
        particles.Add(collision);
    }





    // This function computes circle to circle collisions
    public static CollisionInfo CircleToCircleCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Calculate the distance between both colliders
        Vector2 distance = a.GetPosition() - b.GetPosition();

        bool axisCheck = Vector2.Dot(distance, distance) < (a.GetDimensions().x + b.GetDimensions().x) * (a.GetDimensions().x + b.GetDimensions().x);

        // Are the Radii less than or equal to the distance between both circles?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }


        // Return result
        return new CollisionInfo(axisCheck, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { a.GetDimensions().x + b.GetDimensions().x * (a.GetDimensions().x + b.GetDimensions().x) - Vector2.Dot(distance, distance) });
    }





    // This function computes AABB to AABB collisions
    public static CollisionInfo AABBToAABBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Do an axis check on both the x and y axes
        bool xAxisCheck = a.GetMaximumCorner().x > b.GetMinimumCorner().x && b.GetMaximumCorner().x > a.GetMinimumCorner().x;
        bool yAxisCheck = a.GetMinimumCorner().y < b.GetMaximumCorner().y && b.GetMinimumCorner().y < a.GetMaximumCorner().y;

        // Do the two checks pass?
        if (xAxisCheck && yAxisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        float penetration = 0.0f;
        if (xAxisCheck && yAxisCheck)
        {
            penetration = CollisionResolution.GetAABBPenetration(a, b);
        }


        return new CollisionInfo(xAxisCheck && yAxisCheck, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { penetration });
    }



    // This function computes AABB to OBBB collisions
    public static CollisionInfo AABBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2((Mathf.Cos(b.GetRotation())), Mathf.Abs(-Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2((Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

        List<float> penetration = new List<float>();

        // Do axis checks
        penetration.Add(CollisionResolution.GetBounds(a, b, AUHat));
        bool axisCheck = CollisionResolution.CheckOBBAxis(a, b, AUHat);

        if (!axisCheck)
        {
            return new CollisionInfo(false, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { (a.GetDimensions().x + b.GetDimensions().x * (a.GetDimensions().x + b.GetDimensions().x) - Vector2.Dot(a.GetPosition() - b.GetPosition(), a.GetPosition() - b.GetPosition())) });
        }

        penetration.Add(CollisionResolution.GetBounds(a, b, ARHat));
        axisCheck = CollisionResolution.CheckOBBAxis(a, b, ARHat);

        // Do all checks pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }
        else
        {
            return new CollisionInfo(false, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { a.GetDimensions().x + b.GetDimensions().x * (a.GetDimensions().x + b.GetDimensions().x) - Vector2.Dot(a.GetPosition() - b.GetPosition(), a.GetPosition() - b.GetPosition()) });
        }

        // Return result
        return new CollisionInfo(true, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { CollisionResolution.GetOBBPenetration(penetration) });
    }





    // This function calculates Circle to OBB collisions
    public static CollisionInfo CircleToABBCollision(CollisionHull2D a, CollisionHull2D b)
    {

        Vector2 closestPointToCircle = new Vector2(Math.Max(b.GetMinimumCorner().x, Math.Min(a.GetPosition().x, b.GetMaximumCorner().x)), Math.Max(b.GetMinimumCorner().y, Math.Min(a.GetPosition().y, b.GetMaximumCorner().y)));

        Vector2 distance = a.GetPosition() - closestPointToCircle;
        float distanceSquared = Vector2.Dot(distance, distance);

        bool axisCheck = distanceSquared < a.GetDimensions().x * a.GetDimensions().x;

        // Does the check pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        //float penetration = CollisionResolution.

        // Return result
        return new CollisionInfo(axisCheck, b, a, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() {  (a.GetDimensions().x - Mathf.Sqrt(distanceSquared)) });
    }





    // This function calculate Circle to ABB collisions
    public static CollisionInfo CircleToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        Vector2 closestPointToCircle = new Vector2(Math.Max(b.GetMinimumCorner().x, Math.Min(a.GetPosition().x, b.GetMaximumCorner().x)), Math.Max(b.GetMinimumCorner().y, Math.Min(a.GetPosition().y, b.GetMaximumCorner().y)));

        Vector2 distance = a.GetPosition() - closestPointToCircle;
        float distanceSquared = Vector2.Dot(distance, distance);

        bool axisCheck = distanceSquared < a.GetDimensions().x * a.GetDimensions().x;

        // Does the check pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // return result
        return new CollisionInfo(axisCheck, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { (a.GetDimensions().x - Mathf.Sqrt(distanceSquared)) });
    }




    // This function calculates OBB to OBB colisions
    public static CollisionInfo OBBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for both collision hulls
        Vector2 ARHat = new Vector2(Mathf.Abs(Mathf.Cos(a.GetRotation())), Mathf.Abs(-Mathf.Sin(a.GetRotation())));
        Vector2 BRHat = new Vector2(Mathf.Abs(Mathf.Cos(b.GetRotation())), Mathf.Abs(-Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2(Mathf.Abs(Mathf.Sin(a.GetRotation())), Mathf.Abs(Mathf.Cos(a.GetRotation())));
        Vector2 BUHat = new Vector2(Mathf.Abs(Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

        List<float> penetration = new List<float>();

        // Do axis checks
        penetration.Add(CollisionResolution.GetBounds(a, b, ARHat));
        bool axisChecks = CollisionResolution.CheckOBBAxis(a, b, ARHat);

        

        if (!axisChecks)
        {
            return new CollisionInfo(false, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { a.GetDimensions().x + b.GetDimensions().x * (a.GetDimensions().x + b.GetDimensions().x) - Vector2.Dot(a.GetPosition() - b.GetPosition(), a.GetPosition() - b.GetPosition()) });
        }

        penetration.Add(CollisionResolution.GetBounds(a, b, AUHat));
        axisChecks = CollisionResolution.CheckOBBAxis(a, b, AUHat);

        if (!axisChecks)
        {
            return new CollisionInfo(false, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { a.GetDimensions().x + b.GetDimensions().x * (a.GetDimensions().x + b.GetDimensions().x) - Vector2.Dot(a.GetPosition() - b.GetPosition(), a.GetPosition() - b.GetPosition()) });
        }

        penetration.Add(CollisionResolution.GetBounds(a, b, BRHat));
        axisChecks = CollisionResolution.CheckOBBAxis(a, b, BRHat);

        if (!axisChecks)
        {
            return new CollisionInfo(false, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { a.GetDimensions().x + b.GetDimensions().x * (a.GetDimensions().x + b.GetDimensions().x) - Vector2.Dot(a.GetPosition() - b.GetPosition(), a.GetPosition() - b.GetPosition()) });
        }

        penetration.Add(CollisionResolution.GetBounds(a, b, AUHat));
        axisChecks = CollisionResolution.CheckOBBAxis(a, b, BUHat);

        // Do the axis checks pass?
        if (axisChecks)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }
        else
        {
            return new CollisionInfo(false, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { a.GetDimensions().x + b.GetDimensions().x * (a.GetDimensions().x + b.GetDimensions().x) - Vector2.Dot(a.GetPosition() - b.GetPosition(), a.GetPosition() - b.GetPosition()) });
        }

        // return result
        return new CollisionInfo(true, a, b, CollisionResolution.CalculateSeparatingVelocity(a, b), new List<float>() { CollisionResolution.GetOBBPenetration(penetration) });
    }




    // This function checks for a collision between two objects by projecting onto a specific axis (OBB/Circle Hulls) 
    public static bool CheckOBBAxisForCircle(CollisionHull2D circle, CollisionHull2D OBB, Vector2 rotationAxis)
    {
        // Create a list of all points from the OBB hull
        List<Vector2> shapeAPoints = new List<Vector2>();
        shapeAPoints.Add(new Vector2(OBB.GetPosition().x + OBB.GetDimensions().x, OBB.GetPosition().y + OBB.GetDimensions().y));
        shapeAPoints.Add(new Vector2(OBB.GetPosition().x - OBB.GetDimensions().x, OBB.GetPosition().y - OBB.GetDimensions().y));
        shapeAPoints.Add(new Vector2(OBB.GetPosition().x - OBB.GetDimensions().x, OBB.GetPosition().y + OBB.GetDimensions().y));
        shapeAPoints.Add(new Vector2(OBB.GetPosition().x + OBB.GetDimensions().x, OBB.GetPosition().y - OBB.GetDimensions().y));

        // Initialize min and max of OBB shape
        Vector2 shapeAMin = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 shapeAMax = new Vector2(-Mathf.Infinity, -Mathf.Infinity);

        // Initialize min and max and position of circle hull
        Vector2 circleMin = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 circleMax = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
        Vector2 circlePos = Vector2.Dot(circle.GetPosition(), rotationAxis) * rotationAxis;

        // Initialize all points for axis checks
        for (int i = 0; i < shapeAPoints.Count; i++)
        {
            // Rotate original point
            shapeAPoints[i] = new Vector2(Mathf.Cos(OBB.GetRotation()) * (shapeAPoints[i].x - OBB.GetPosition().x) - Mathf.Sin(OBB.GetRotation()) * (shapeAPoints[i].y - OBB.GetPosition().y) + OBB.GetPosition().x,
                                          Mathf.Sin(OBB.GetRotation()) * (shapeAPoints[i].x - OBB.GetPosition().x) + Mathf.Cos(OBB.GetRotation()) * (shapeAPoints[i].y - OBB.GetPosition().y) + OBB.GetPosition().y);

            // Project the point onto the rotation axis
            shapeAPoints[i] = Vector2.Dot(shapeAPoints[i], rotationAxis) * rotationAxis;
        }

        // Iterate through all points to find the minimum and maximum points
        for (int i = 0; i < shapeAPoints.Count; i++)
        {
            // Is the current point less than the current minimum?
            if (shapeAPoints[i].x <= shapeAMin.x && shapeAPoints[i].y <= shapeAMin.y)
            {
                // If yes, then make this point the new min
                shapeAMin = shapeAPoints[i];
            }

            // Is the current point less than the current maximum?
            if (shapeAPoints[i].x >= shapeAMax.x && shapeAPoints[i].y >= shapeAMax.y)
            {
                // If yes, then make this point the new max
                shapeAMax = shapeAPoints[i];
            }
        }

        // Calculate the minimum and maximum of the circle hull
        circleMin = circlePos + (rotationAxis * circle.GetDimensions().x * -1);
        circleMax = circlePos + (rotationAxis * circle.GetDimensions().x);


        // Do axis checks
        bool xAxisCheck = shapeAMin.x < circleMax.x && circleMin.x < shapeAMax.x;
        bool yAxisCheck = shapeAMin.y < circleMax.y && circleMin.y < shapeAMax.y;

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
