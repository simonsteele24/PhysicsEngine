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
            public Vector2 point;
            public Vector2 normal;
            public float restitution;
        }
        public CollisionHull2D a;
        public CollisionHull2D b;
        public Contact[] contacts = new Contact[4];

        public float separatingVelocity;
        public float penetration;

        public bool status;


        public CollisionInfo(bool _status, CollisionHull2D _a, CollisionHull2D _b, float _separatingVelocity, float penetrationValue)
        {
            status = _status;
            a = _a;
            b = _b;
            separatingVelocity = _separatingVelocity;
            Vector2 contactNormal = (_b.GetPosition() - _a.GetPosition()).normalized;
            contacts[0].point = FindPointOfContactWithCircle(_a, _b);
            contacts[0].normal = contactNormal;
            contacts[0].restitution = 1;
            penetration = penetrationValue;
        }

    }


    public static CollisionManager manager;

    private Dictionary<CollisionPairKey, Func<CollisionHull2D, CollisionHull2D, CollisionInfo>> _collisionTypeCollisionTestFunctions = new Dictionary<CollisionPairKey, Func<CollisionHull2D, CollisionHull2D, CollisionInfo>>(new CollisionPairKey.EqualityComparitor());



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
        //_collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.AABB, CollisionHullType2D.AABB), AABBToAABBCollision);
        //_collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.OBBB, CollisionHullType2D.OBBB), OBBToOBBCollision);
        //_collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.Circle, CollisionHullType2D.OBBB), CircleToOBBCollision);
        //_collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.Circle, CollisionHullType2D.AABB), CircleToABBCollision);
        //_collisionTypeCollisionTestFunctions.Add(new CollisionPairKey(CollisionHullType2D.AABB, CollisionHullType2D.OBBB), AABBToOBBCollision);
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
                        if (collisions[i].a == particles[y] && collisions[i].b == particles[x])
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
        ResolveCollisions(collisions);
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

        Debug.Log(Vector2.Dot(distance, distance));
        Debug.Log((a.GetDimensions().x + b.GetDimensions().x) * (a.GetDimensions().x + b.GetDimensions().x));
        bool axisCheck = Vector2.Dot(distance, distance) <= (a.GetDimensions().x + b.GetDimensions().x) * (a.GetDimensions().x + b.GetDimensions().x);

        // Are the Radii less than or equal to the distance between both circles?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // Return result
        return new CollisionInfo(axisCheck, a, b, CalculateSeparatingVelocity(a, b), (a.GetDimensions().x + b.GetDimensions().x) * (a.GetDimensions().x + b.GetDimensions().x) - Vector2.Dot(distance,distance));
    }





    /*// This function computes AABB to AABB collisions
    public static CollisionInfo AABBToAABBCollision(CollisionHull2D a, CollisionHull2D b)
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
        return new CollisionInfo(xAxisCheck && yAxisCheck, a, b, CalculateSeparatingVelocity(a, b));
    }





    // This function computes AABB to OBBB collisions
    public static CollisionInfo AABBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2((Mathf.Cos(b.GetRotation())), Mathf.Abs(-Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2((Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

        // Do axis checks
        bool axisCheck = CheckOBBAxis(a, b, AUHat);

        if (!axisCheck)
        {
            return new CollisionInfo(false, a, b, CalculateSeparatingVelocity(a, b));
        }

        axisCheck = CheckOBBAxis(a, b, ARHat);

        // Do all checks pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }
        else
        {
            return new CollisionInfo(false, a, b, CalculateSeparatingVelocity(a, b));
        }

        // Return result
        return new CollisionInfo(true, a, b, CalculateSeparatingVelocity(a, b));
    }





    // This function calculates Circle to OBB collisions
    public static CollisionInfo CircleToABBCollision(CollisionHull2D a, CollisionHull2D b)
    {

        Vector2 closestPointToCircle = new Vector2(Math.Max(b.GetMinimumCorner().x, Math.Min(a.GetPosition().x, b.GetMaximumCorner().x)), Math.Max(b.GetMinimumCorner().y, Math.Min(a.GetPosition().y, b.GetMaximumCorner().y)));

        Vector2 distance = a.GetPosition() - closestPointToCircle;

        bool axisCheck = Vector2.Dot(distance, distance) <= a.GetDimensions().x * a.GetDimensions().x;

        // Does the check pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }

        // Return result
        return new CollisionInfo(axisCheck, a, b, CalculateSeparatingVelocity(a, b));
    }





    // This function calculate Circle to ABB collisions
    public static CollisionInfo CircleToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for A
        Vector2 ARHat = new Vector2((Mathf.Cos(b.GetRotation())), Mathf.Abs(-Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2((Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

        // Do axis checks
        bool axisCheck = CheckOBBAxisForCircle(a, b, ARHat);

        if (!axisCheck)
        {
            return new CollisionInfo(false, a, b, CalculateSeparatingVelocity(a, b));
        }

        axisCheck = CheckOBBAxisForCircle(a, b, AUHat);

        // Do all checks pass?
        if (axisCheck)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }
        else
        {
            return new CollisionInfo(false, a, b, CalculateSeparatingVelocity(a, b));
        }

        // return result
        return new CollisionInfo(true, a, b, CalculateSeparatingVelocity(a, b));
    }





    // This function calculates OBB to OBB colisions
    public static CollisionInfo OBBToOBBCollision(CollisionHull2D a, CollisionHull2D b)
    {
        // Compute the R hat and U hat for both collision hulls
        Vector2 ARHat = new Vector2(Mathf.Abs(Mathf.Cos(a.GetRotation())), Mathf.Abs(-Mathf.Sin(a.GetRotation())));
        Vector2 BRHat = new Vector2(Mathf.Abs(Mathf.Cos(b.GetRotation())), Mathf.Abs(-Mathf.Sin(b.GetRotation())));
        Vector2 AUHat = new Vector2(Mathf.Abs(Mathf.Sin(a.GetRotation())), Mathf.Abs(Mathf.Cos(a.GetRotation())));
        Vector2 BUHat = new Vector2(Mathf.Abs(Mathf.Sin(b.GetRotation())), Mathf.Abs(Mathf.Cos(b.GetRotation())));

        // Do axis checks
        bool axisChecks = CheckOBBAxis(a, b, ARHat);

        if (!axisChecks)
        {
            return new CollisionInfo(false, a, b, CalculateSeparatingVelocity(a, b));
        }

        axisChecks = CheckOBBAxis(a, b, AUHat);

        if (!axisChecks)
        {
            return new CollisionInfo(false, a, b, CalculateSeparatingVelocity(a, b));
        }

        axisChecks = CheckOBBAxis(a, b, BRHat);

        if (!axisChecks)
        {
            return new CollisionInfo(false, a, b, CalculateSeparatingVelocity(a, b));
        }

        axisChecks = CheckOBBAxis(a, b, BUHat);

        // Do the axis checks pass?
        if (axisChecks)
        {
            // If yes, then inform the parents of the complex shape object (if applicable)
            ReportCollisionToParent(a, b);
        }
        else
        {
            return new CollisionInfo(false, a, b, CalculateSeparatingVelocity(a, b));
        }

        // return result
        return new CollisionInfo(true, a, b, CalculateSeparatingVelocity(a, b));
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
        bool xAxisCheck = shapeAMin.x <= circleMax.x && circleMin.x <= shapeAMax.x;
        bool yAxisCheck = shapeAMin.y <= circleMax.y && circleMin.y <= shapeAMax.y;

        // Return result
        return xAxisCheck && yAxisCheck;
    }





    // This function checks for a collision between two objects by projecting onto a specific axis
    public static bool CheckOBBAxis(CollisionHull2D shapeA, CollisionHull2D shapeB, Vector2 rotationAxis)
    {
        // Create a list of all points from the OBB hull for shape A
        List<Vector2> shapeAPoints = new List<Vector2>();
        shapeAPoints.Add(new Vector2(shapeA.GetPosition().x + shapeA.GetDimensions().x, shapeA.GetPosition().y + shapeA.GetDimensions().y));
        shapeAPoints.Add(new Vector2(shapeA.GetPosition().x - shapeA.GetDimensions().x, shapeA.GetPosition().y - shapeA.GetDimensions().y));
        shapeAPoints.Add(new Vector2(shapeA.GetPosition().x - shapeA.GetDimensions().x, shapeA.GetPosition().y + shapeA.GetDimensions().y));
        shapeAPoints.Add(new Vector2(shapeA.GetPosition().x + shapeA.GetDimensions().x, shapeA.GetPosition().y - shapeA.GetDimensions().y));

        // Create a list of all points from the OBB hull for shape B
        List<Vector2> shapeBPoints = new List<Vector2>();
        shapeBPoints.Add(new Vector2(shapeB.GetPosition().x + shapeB.GetDimensions().x, shapeB.GetPosition().y + shapeB.GetDimensions().y));
        shapeBPoints.Add(new Vector2(shapeB.GetPosition().x - shapeB.GetDimensions().x, shapeB.GetPosition().y - shapeB.GetDimensions().y));
        shapeBPoints.Add(new Vector2(shapeB.GetPosition().x - shapeB.GetDimensions().x, shapeB.GetPosition().y + shapeB.GetDimensions().y));
        shapeBPoints.Add(new Vector2(shapeB.GetPosition().x + shapeB.GetDimensions().x, shapeB.GetPosition().y - shapeB.GetDimensions().y));

        // Initialize min and max of OBB shape
        Vector2 shapeAMin = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 shapeAMax = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
        Vector2 shapeBMin = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 shapeBMax = new Vector2(-Mathf.Infinity, -Mathf.Infinity);

        // Initialize all points for axis checks
        for (int i = 0; i < shapeAPoints.Count; i++)
        {
            // Rotate original point
            shapeAPoints[i] = new Vector2(Mathf.Cos(shapeA.GetRotation()) * (shapeAPoints[i].x - shapeA.GetPosition().x) - Mathf.Sin(shapeA.GetRotation()) * (shapeAPoints[i].y -shapeA.GetPosition().y) + shapeA.GetPosition().x,
                                          Mathf.Sin(shapeA.GetRotation()) * (shapeAPoints[i].x - shapeA.GetPosition().x) + Mathf.Cos(shapeA.GetRotation()) * (shapeAPoints[i].y - shapeA.GetPosition().y) + shapeA.GetPosition().y);

            // Project the point onto the rotation axis
            shapeAPoints[i] = Vector2.Dot(shapeAPoints[i], rotationAxis) * rotationAxis;

            // Rotate original point
            shapeBPoints[i] = new Vector2(Mathf.Cos(shapeB.GetRotation()) * (shapeBPoints[i].x - shapeB.GetPosition().x) - Mathf.Sin(shapeB.GetRotation()) * (shapeBPoints[i].y - shapeB.GetPosition().y) + shapeB.GetPosition().x,
                                          Mathf.Sin(shapeB.GetRotation()) * (shapeBPoints[i].x - shapeB.GetPosition().x) + Mathf.Cos(shapeB.GetRotation()) * (shapeBPoints[i].y - shapeB.GetPosition().y) + shapeB.GetPosition().y);

            // Project the point onto the rotation axis
            shapeBPoints[i] = Vector2.Dot(shapeBPoints[i], rotationAxis) * rotationAxis;
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

            // Is the current point more than the current maximum?
            if (shapeAPoints[i].x >= shapeAMax.x && shapeAPoints[i].y >= shapeAMax.y)
            {
                // If yes, then make this point the new max
                shapeAMax = shapeAPoints[i];
            }

            // Is the current point less than the current minimum?
            if (shapeBPoints[i].x <= shapeBMin.x && shapeBPoints[i].y <= shapeBMin.y)
            {
                // If yes, then make this point the new min
                shapeBMin = shapeBPoints[i];
            }

            // Is the current point more than the current maximum?
            if (shapeBPoints[i].x >= shapeBMax.x && shapeBPoints[i].y >= shapeBMax.y)
            {
                // If yes, then make this point the new max
                shapeBMax = shapeBPoints[i];
            }
        }

        // Do axis checks
        bool xAxisCheck = shapeAMin.x <= shapeBMax.x && shapeBMin.x <= shapeAMax.x;
        bool yAxisCheck = shapeAMin.y <= shapeBMax.y && shapeBMin.y <= shapeAMax.y;

        // Return result
        return xAxisCheck && yAxisCheck;
    }*/





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



    public static float CalculateSeparatingVelocity(CollisionHull2D shapeA, CollisionHull2D shapeB)
    {
        //(velocity of a - velocity of b) * normalized(position of a - position of b)
        Vector2 differenceOfVelocity = (shapeA.gameObject.GetComponent<Particle2D>().velocity - shapeB.gameObject.GetComponent<Particle2D>().velocity) * -1;
        Vector2 differenceOfPosition = (shapeA.GetPosition() - shapeB.GetPosition()).normalized;

        return Vector2.Dot(differenceOfVelocity, differenceOfPosition);
    }


    public static Vector2 FindPointOfContactWithCircle(CollisionHull2D circleA, CollisionHull2D circleB)
    {
        Vector2 ratioA = (circleA.GetDimensions().x / (circleA.GetDimensions().x + circleB.GetDimensions().x)) * circleA.GetPosition();
        Vector2 ratioB = (circleB.GetDimensions().x / (circleA.GetDimensions().x + circleB.GetDimensions().x)) * circleB.GetPosition();
        return ratioA + ratioB;
    }



    public static void ResolveCollisions(List<CollisionInfo> collisions)
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            ResolvePenetration(collisions[i]);
            ResolveVelocities(collisions[i]);
        }
    }



    public static void ResolveVelocities(CollisionInfo collision)
    {
        if (!collision.status)
        {
            return;
        }

        float newSeperatingVelocity = -collision.separatingVelocity * collision.contacts[0].restitution;
        float deltaVelocity = newSeperatingVelocity - collision.separatingVelocity;

        float totalInverseMass = collision.a.GetComponent<Particle2D>().invMass;
        totalInverseMass += collision.b.GetComponent<Particle2D>().invMass;

        if (totalInverseMass <= 0)
        {
            return;
        }

        float impulse = deltaVelocity / totalInverseMass;

        Vector2 impulsePerIMass = collision.contacts[0].normal * impulse;

        collision.a.GetComponent<Particle2D>().velocity = collision.a.GetComponent<Particle2D>().velocity + impulsePerIMass * collision.a.GetComponent<Particle2D>().invMass;
        collision.b.GetComponent<Particle2D>().velocity = collision.b.GetComponent<Particle2D>().velocity + impulsePerIMass * -collision.b.GetComponent<Particle2D>().invMass;

    }

    public static void ResolvePenetration(CollisionInfo collision)
    {
        if (collision.penetration <= 0) { return; }

        float totalInvMass = collision.a.GetComponent<Particle2D>().invMass;
        totalInvMass += collision.b.GetComponent<Particle2D>().invMass;

        if (totalInvMass <= 0)
        {
            return;
        }

        Vector2 movePerIMass = collision.contacts[0].normal * (collision.penetration / totalInvMass);

        Vector3 particleMovementA = -movePerIMass * collision.a.GetComponent<Particle2D>().invMass;
        Vector3 particleMovementB = movePerIMass * collision.b.GetComponent<Particle2D>().invMass;

        collision.a.transform.position += particleMovementA;
        collision.a.GetComponent<Particle2D>().position = collision.a.transform.position;
        collision.a.SetPosition(collision.a.transform.position);

        collision.b.transform.position += particleMovementB;
        collision.b.GetComponent<Particle2D>().position = collision.b.transform.position;
        collision.b.SetPosition(collision.b.transform.position);

        Debug.Log("Initial penetration: " + collision.penetration);
        Debug.Log("Resulting Penetration: " + ((collision.a.GetDimensions().x + collision.b.GetDimensions().x) * (collision.a.GetDimensions().x + collision.b.GetDimensions().x) - Vector2.Dot((collision.a.GetPosition() - collision.b.GetPosition()), (collision.a.GetPosition() - collision.b.GetPosition()))));
    }
}
