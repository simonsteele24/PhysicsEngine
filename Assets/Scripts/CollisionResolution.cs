using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResolution : MonoBehaviour
{
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

        float shapeAMin = Mathf.Infinity;
        float shapeAMax = -Mathf.Infinity;
        float shapeBMin = Mathf.Infinity;
        float shapeBMax = -Mathf.Infinity;

        // Initialize all points for axis checks
        for (int i = 0; i < shapeAPoints.Count; i++)
        {
            // Rotate original point
            shapeAPoints[i] = new Vector2(Mathf.Cos(shapeA.GetRotation()) * (shapeAPoints[i].x - shapeA.GetPosition().x) - Mathf.Sin(shapeA.GetRotation()) * (shapeAPoints[i].y - shapeA.GetPosition().y) + shapeA.GetPosition().x,
                                          Mathf.Sin(shapeA.GetRotation()) * (shapeAPoints[i].x - shapeA.GetPosition().x) + Mathf.Cos(shapeA.GetRotation()) * (shapeAPoints[i].y - shapeA.GetPosition().y) + shapeA.GetPosition().y);
            float temp = Vector2.Dot(shapeAPoints[i], rotationAxis);

            if (temp < shapeAMin)
            {
                shapeAMin = temp;
            }

            if (temp > shapeAMax)
            {
                shapeAMax = temp;
            }

            // Rotate original point
            shapeBPoints[i] = new Vector2(Mathf.Cos(shapeB.GetRotation()) * (shapeBPoints[i].x - shapeB.GetPosition().x) - Mathf.Sin(shapeB.GetRotation()) * (shapeBPoints[i].y - shapeB.GetPosition().y) + shapeB.GetPosition().x,
                                          Mathf.Sin(shapeB.GetRotation()) * (shapeBPoints[i].x - shapeB.GetPosition().x) + Mathf.Cos(shapeB.GetRotation()) * (shapeBPoints[i].y - shapeB.GetPosition().y) + shapeB.GetPosition().y);

            temp = Vector2.Dot(shapeBPoints[i], rotationAxis);

            if (temp < shapeBMin)
            {
                shapeBMin = temp;
            }

            if (temp > shapeBMax)
            {
                shapeBMax = temp;
            }
        }

        float totalMin;
        float totalMax;
        if (shapeBMin > shapeAMin)
        {
            totalMin = shapeAMin;
        }
        else
        {
            totalMin = shapeBMin;
        }
        if (shapeBMax > shapeAMax)
        {
            totalMax = shapeAMax;
        }
        else
        {
            totalMax = shapeBMax;
        }

        // Do axis checks
        bool axisCheck = shapeAMin <= shapeBMax && shapeBMin <= shapeAMax;

        // Return result
        return axisCheck;
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

        // Find the distance between both objects
        Vector2 n = (shapeBPos - shapeAPos);

        // Calculate overlap on x axis
        float x_overlap = a_extent + b_extent - Mathf.Abs(n.x);

        // get the extents of each object
        a_extent = (shapeAMax.x - shapeAMin.x) / 2.0f;
        b_extent = (shapeAMax.y - shapeAMin.y) / 2.0f;

        // Calculate the overlap on y axis
        float y_overlap = a_extent + b_extent - Mathf.Abs(n.y);

        // Is the overlap on X greater than that on Y
        if (x_overlap > y_overlap)
        {
            // If yes, then return Y
            return y_overlap;
        }
        else
        {
            // If no, then return X
            return x_overlap;
        }
    }





    // This function gets the seperating velocity of two particles
    public static float CalculateSeparatingVelocity(CollisionHull2D shapeA, CollisionHull2D shapeB)
    {
        // Find all required values for calculation
        Vector2 differenceOfVelocity = (shapeA.gameObject.GetComponent<Particle2D>().velocity - shapeB.gameObject.GetComponent<Particle2D>().velocity) * -1;
        Vector2 differenceOfPosition = (shapeA.GetPosition() - shapeB.GetPosition()).normalized;

        // Return the dot product of both velocity and position
        return Vector2.Dot(differenceOfVelocity, differenceOfPosition);
    }





    // This function goes through all collisions and resovlves each and every one of them
    public static void ResolveCollisions(List<CollisionManager.CollisionInfo> collisions, float dt)
    {
        for (int i = 0; i < collisions.Count; i++)
        {
            // Are the two particles moving towards each other
            if (collisions[i].separatingVelocity > 0)
            {
                // If yes, then resolve collision
                ResolvePenetration(collisions[i]);
                ResolveVelocities(collisions[i], dt);
            }
        }
    }





    // This function resolves the velocities with two particles in a collision
    public static void ResolveVelocities(CollisionManager.CollisionInfo collision, float dt)
    {
        // Get the new seperating velocity
        float newSeperatingVelocity = -collision.separatingVelocity * CollisionManager.UNIVERSAL_COEFFICIENT_OF_RESTITUTION;

        // Check the velocity buildup due to acceleration only.
        Vector2 accCausedVelocity = collision.a.GetComponent<Particle2D>().acceleration - collision.b.GetComponent<Particle2D>().acceleration;
        float accCausedSepVelocity = Vector2.Dot(accCausedVelocity, collision.normal) * Time.fixedDeltaTime;



        // If we’ve got a closing velocity due to aceleration buildup,
        // remove it from the new separating velocity.
        if (accCausedSepVelocity < 0)
        {
            newSeperatingVelocity *= accCausedSepVelocity;
            if (newSeperatingVelocity < 0) newSeperatingVelocity = 0;
        }


        // Get the delta velocity between the new and old seperating velocity
        float deltaVelocity = newSeperatingVelocity - collision.separatingVelocity;

        // Get the total inverse mass
        float totalInverseMass = collision.a.GetComponent<Particle2D>().invMass;
        totalInverseMass += collision.b.GetComponent<Particle2D>().invMass;

        // Do both particles have an infinite mass?
        if (totalInverseMass <= 0)
        {
            // If yes, exit the function
            return;
        }

        // Get the impulse of the collision
        float impulse = deltaVelocity / totalInverseMass;
        Vector2 impulsePerIMass = collision.normal * impulse;

        // Apply the new velocities to both particles
        collision.a.GetComponent<Particle2D>().velocity = collision.a.GetComponent<Particle2D>().velocity + impulsePerIMass * collision.a.GetComponent<Particle2D>().invMass;
        collision.b.GetComponent<Particle2D>().velocity = collision.b.GetComponent<Particle2D>().velocity + impulsePerIMass * -collision.b.GetComponent<Particle2D>().invMass;

    }





    // This function resolves the penetration of a collision
    public static void ResolvePenetration(CollisionManager.CollisionInfo collision)
    {
        // Vector3's
        Vector3 particleMovementA;
        Vector3 particleMovementB;

        // If the penetration is non-positive, do not attempt to resolve the penetration
        if (collision.penetration <= 0) { return; }

        // Find the total inverse mass
        float totalInvMass = collision.a.GetComponent<Particle2D>().invMass;
        totalInvMass += collision.b.GetComponent<Particle2D>().invMass;

        // Do both particles have an infinite mass?
        if (totalInvMass <= 0)
        {
            // If yes, then exit the function
            return;
        }

        // Calculate amount each object needs to move to resolve penetration
        Vector2 movePerIMass = collision.normal * (collision.penetration / totalInvMass);

        // Is particle A's position less than B's position
        if (collision.a.GetPosition().y < collision.b.GetPosition().y)
        {
            // Determine the amount both object needs to move
            particleMovementA = movePerIMass * collision.a.GetComponent<Particle2D>().invMass;
            particleMovementB = -movePerIMass * collision.b.GetComponent<Particle2D>().invMass;
        }
        else
        {
            // Determine the amount both object needs to move
            particleMovementA = -movePerIMass * collision.a.GetComponent<Particle2D>().invMass;
            particleMovementB = movePerIMass * collision.b.GetComponent<Particle2D>().invMass;
        }

        // Apply movement to particle A
        collision.a.transform.position += particleMovementA;
        collision.a.GetComponent<Particle2D>().position = collision.a.transform.position;
        collision.a.SetPosition(collision.a.transform.position);

        // Apply movement to particle B
        collision.b.transform.position += particleMovementB;
        collision.b.GetComponent<Particle2D>().position = collision.b.transform.position;
        collision.b.SetPosition(collision.b.transform.position);
    }





    // This function gets the penetration based on overlap values
    public static float GetFinalPenetration(List<float> overlaps)
    {
        // Initialize values
        float penetration = -Mathf.Infinity;


        for (int i = 0; i < overlaps.Count; i++)
        {
            // Is the overlap less than the current penetration?
            if (overlaps[i] < penetration && overlaps[i] >= 0)
            {
                // If yes, then set the penetration to this value
                penetration = overlaps[i];
            }
        }

        // Return the result
        return penetration;
    }
}
