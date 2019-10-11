﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResolution : MonoBehaviour
{
    public static float GetOBBCircleBounds(CollisionHull2D circle, CollisionHull2D OBB, Vector2 rotationAxis)
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
        Vector2 shapeAPos = Vector2.Dot(OBB.GetPosition(), rotationAxis) * rotationAxis;

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

        // Calculate half extents along x axis for each object
        float a_extent = (shapeAMax.x - shapeAMin.x) / 2.0f;
        float b_extent = (circleMax.x - circleMin.x) / 2.0f;

        Vector2 n = (circlePos - shapeAPos);

        // Calculate overlap on x axis
        float x_overlap = a_extent + b_extent - Mathf.Abs(n.x);

        a_extent = (shapeAMax.x - shapeAMin.x) / 2.0f;
        b_extent = (shapeAMax.y - shapeAMin.y) / 2.0f;

        float y_overlap = a_extent + b_extent - Mathf.Abs(n.y);

        if (x_overlap > y_overlap)
        {
            return x_overlap;
        }
        else
        {
            return y_overlap;
        }
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

        Vector2 shapeAPos;
        Vector2 shapeBPos;

        // Initialize all points for axis checks
        for (int i = 0; i < shapeAPoints.Count; i++)
        {
            // Rotate original point
            shapeAPoints[i] = new Vector2(Mathf.Cos(shapeA.GetRotation()) * (shapeAPoints[i].x - shapeA.GetPosition().x) - Mathf.Sin(shapeA.GetRotation()) * (shapeAPoints[i].y - shapeA.GetPosition().y) + shapeA.GetPosition().x,
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
    }



    public static float GetBounds(CollisionHull2D shapeA, CollisionHull2D shapeB, Vector2 rotationAxis)
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

        Vector2 shapeAPos = Vector2.Dot(shapeA.GetPosition(), rotationAxis) * rotationAxis;
        Vector2 shapeBPos = Vector2.Dot(shapeB.GetPosition(), rotationAxis) * rotationAxis;

        // Initialize all points for axis checks
        for (int i = 0; i < shapeAPoints.Count; i++)
        {
            // Rotate original point
            shapeAPoints[i] = new Vector2(Mathf.Cos(shapeA.GetRotation()) * (shapeAPoints[i].x - shapeA.GetPosition().x) - Mathf.Sin(shapeA.GetRotation()) * (shapeAPoints[i].y - shapeA.GetPosition().y) + shapeA.GetPosition().x,
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

        // Calculate half extents along x axis for each object
        float a_extent = (shapeAMax.x - shapeAMin.x) / 2.0f;
        float b_extent = (shapeBMax.x - shapeBMin.x) / 2.0f;

        Vector2 n = (shapeBPos - shapeAPos);

        // Calculate overlap on x axis
        float x_overlap = a_extent + b_extent - Mathf.Abs(n.x);

        a_extent = (shapeAMax.x - shapeAMin.x) / 2.0f;
        b_extent = (shapeAMax.y - shapeAMin.y) / 2.0f;

        float y_overlap = a_extent + b_extent - Mathf.Abs(n.y);

        if (x_overlap > y_overlap)
        {
            return x_overlap;
        }
        else
        {
            return y_overlap;
        }
    }



    



    public static float CalculateSeparatingVelocity(CollisionHull2D shapeA, CollisionHull2D shapeB)
    {
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



    public static void ResolveCollisions(List<CollisionManager.CollisionInfo> collisions, float dt)
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            ResolvePenetration(collisions[i]);
            ResolveVelocities(collisions[i],dt);
        }
    }



    public static void ResolveVelocities(CollisionManager.CollisionInfo collision, float dt)
    {
        if (!collision.status)
        {
            return;
        }

        float newSeperatingVelocity = -collision.separatingVelocity * collision.a.GetComponent<Particle2D>().restitiution;

        /*// Check the velocity buildup due to acceleration only.
        Vector2 accCausedVelocity = collision.a.GetComponent<Particle2D>().velocity += collision.b.GetComponent<Particle2D>().velocity;



        float accCausedSepVelocity = Vector2.Dot(accCausedVelocity, accCausedVelocity) / 2.0f;
        accCausedSepVelocity = Mathf.Abs(accCausedSepVelocity);

        if (accCausedSepVelocity < CollisionManager.RESTING_CONTACT_VALUE)
        {
            Debug.Log("Here!");
            newSeperatingVelocity = 0;
        }*/



        float deltaVelocity = newSeperatingVelocity - collision.separatingVelocity;

        float totalInverseMass = collision.a.GetComponent<Particle2D>().invMass;
        totalInverseMass += collision.b.GetComponent<Particle2D>().invMass;

        if (totalInverseMass <= 0)
        {
            return;
        }

        float impulse = deltaVelocity / totalInverseMass;

        Vector2 impulsePerIMass = collision.normal * impulse;

        collision.a.GetComponent<Particle2D>().velocity = collision.a.GetComponent<Particle2D>().velocity + impulsePerIMass * collision.a.GetComponent<Particle2D>().invMass;
        collision.b.GetComponent<Particle2D>().velocity = collision.b.GetComponent<Particle2D>().velocity + impulsePerIMass * -collision.b.GetComponent<Particle2D>().invMass;

    }

    public static void ResolvePenetration(CollisionManager.CollisionInfo collision)
    {
        if (collision.penetration <= 0) { return; }

        float totalInvMass = collision.a.GetComponent<Particle2D>().invMass;
        totalInvMass += collision.b.GetComponent<Particle2D>().invMass;

        if (totalInvMass <= 0)
        {
            return;
        }

        Vector2 movePerIMass = collision.normal * (collision.penetration / totalInvMass);

        Vector3 particleMovementA;
        Vector3 particleMovementB;

        if (collision.a.GetPosition().x > collision.b.GetPosition().x)
        {
            particleMovementA = movePerIMass * collision.a.GetComponent<Particle2D>().invMass;
            particleMovementB = -movePerIMass * collision.b.GetComponent<Particle2D>().invMass;
        }
        else
        {
            particleMovementA = -movePerIMass * collision.a.GetComponent<Particle2D>().invMass;
            particleMovementB = movePerIMass * collision.b.GetComponent<Particle2D>().invMass;
        }

        collision.a.transform.position += particleMovementA;
        collision.a.GetComponent<Particle2D>().position = collision.a.transform.position;
        collision.a.SetPosition(collision.a.transform.position);

        collision.b.transform.position += particleMovementB;
        collision.b.GetComponent<Particle2D>().position = collision.b.transform.position;
        collision.b.SetPosition(collision.b.transform.position);
    }




    public static float GetAABBPenetration(CollisionHull2D a, CollisionHull2D b)
    {
        // Calculate half extents along x axis for each object
        float a_extent = (a.GetMaximumCorner().x - a.GetMinimumCorner().x) / 2.0f;
        float b_extent = (b.GetMaximumCorner().x - b.GetMinimumCorner().x) / 2.0f;

        Vector2 n = (b.GetPosition() - a.GetPosition());

        // Calculate overlap on x axis
        float x_overlap = a_extent + b_extent - Mathf.Abs(n.x);

        // SAT test on x axis
        if (x_overlap > 0)
        {
            // Calculate half extents along x axis for each object
            a_extent = (a.GetMaximumCorner().y - a.GetMinimumCorner().y) / 2.0f;
            b_extent = (b.GetMaximumCorner().y - b.GetMinimumCorner().y) / 2.0f;

            // Calculate overlap on y axis
            float y_overlap = a_extent + b_extent - Mathf.Abs(n.y);

            // SAT test on y axis
            if (y_overlap > 0)
            {
                // Find out which axis is axis of least penetration
                if (x_overlap > y_overlap)
                {
                    return x_overlap;
                }
                else
                {
                    return y_overlap;
                }
            }
        }
        return 0.0f;
    }

    public static float GetOBBPenetration(List<float> penetrations)
    {
        float penetration = -Mathf.Infinity;
        for (int i = 0; i < penetrations.Count; i++)
        {
            if (penetrations[i] > penetration)
            {
                penetration = penetrations[i];
            }
        }
        return penetration;
    }
}