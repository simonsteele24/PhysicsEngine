/*
Author: Simon Steele
Class: GPR-350-101
Assignment: Lab 1
Certification of Authenticity: We certify that this
assignment is entirely our own work.
*/

using UnityEngine;

public class Particle2D : MonoBehaviour
{
    // Locomotion System Enum variables
    public LocomotionSystemType LocomotionSystem;

    // Vector2's
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;

    // Floats
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;




    private void FixedUpdate()
    {
        // Has the user chosen to use the Euler Explicit Locomotion system?
        if (LocomotionSystem == LocomotionSystemType.EulerExplicit)
        {
            // If yes, then apply all explicit formulas for rotation and translation
            updateRotationEulerExplicit(Time.fixedDeltaTime);
            updatePositionEulerExplicit(Time.fixedDeltaTime);
        }
        else
        {
            // If no, then apply all kinematic formulas for rotation and translation
            updatePositionKinematic(Time.deltaTime);
            updateRotationKinematic(Time.deltaTime);
        }

        // Change position to the positional variables
        transform.position = position;

        // Change rotation to the rotational variables
        transform.eulerAngles = new Vector3(0, 0, rotation);

        // Demonstrate movement of particle
        DemonstrateParticleMovement();
    }



    // The following function calculates the new position of the
    // particle using the Explicit Locomotion system
    void updatePositionEulerExplicit(float deltaTime)
    {
        position += velocity * deltaTime;
        velocity += acceleration * deltaTime;
    }



    // The following function calculates the new position of the
    // particle using the Kinematic Locomotion system
    void updatePositionKinematic(float deltaTime)
    {
        position += velocity * deltaTime + 0.5f * acceleration * Mathf.Pow(deltaTime, 2);
        velocity += acceleration * deltaTime;
    }



    // The following function calculates the new rotation of the
    // particle using the Explicit Locomotion system
    void updateRotationEulerExplicit(float deltaTime)
    {
        rotation += angularVelocity * deltaTime;
        angularVelocity += angularAcceleration * deltaTime;
    }



    // The following function calculates the new rotation of the
    // particle using the Kinematic Locomotion system
    void updateRotationKinematic(float deltaTime)
    {
        rotation += angularVelocity * deltaTime + 0.5f * angularAcceleration * Mathf.Pow(deltaTime, 2);

        angularVelocity += angularAcceleration * deltaTime;
    }



    // The following function attempts to show off what the physics engine can do
    // If you want to make changes how the particle moves, do it here
    void DemonstrateParticleMovement()
    {
        acceleration.x = -Mathf.Sin(Time.time);
        acceleration.y = -Mathf.Cos(Time.time);
    }
}
