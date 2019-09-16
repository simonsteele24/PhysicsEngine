/*
Author: Simon Steele
Class: GPR-350-101
Assignment: Lab 2
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

    const float GRAV_CONSTANT = -10;

    // Floats
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;

    [Range(0, Mathf.Infinity)]
    public float mass;

    private Vector2 force;

    private float invMass;

    private float Mass
    {
        set
        {
            mass = mass > 0.0f ? mass: 0.0f;
            invMass = mass > 0.0f ? 1.0f / mass : 0.0f;
        }

        get
        {
            return mass;
        }
    }


    private void Start()
    {
        Mass = mass;
    }

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

        UpdateAcceleration();

        // Change position to the positional variables
        transform.position = position;

        // Change rotation to the rotational variables
        transform.eulerAngles = new Vector3(0, 0, rotation);

        // Demonstrate movement of particle
        //DemonstrateParticleMovement();
    }



    // The following function calculates the new position of the
    // particle using the Explicit Locomotion system
    void updatePositionEulerExplicit(float dt)
    {
        position += velocity * dt;
        velocity += acceleration * dt;
    }



    // The following function calculates the new position of the
    // particle using the Kinematic Locomotion system
    void updatePositionKinematic(float dt)
    {
        position += velocity * dt + 0.5f * acceleration * Mathf.Pow(dt, 2);
        velocity += acceleration * dt;
    }



    // The following function calculates the new rotation of the
    // particle using the Explicit Locomotion system
    void updateRotationEulerExplicit(float dt)
    {
        rotation += angularVelocity * dt;
        angularVelocity += angularAcceleration * dt;
    }



    // The following function calculates the new rotation of the
    // particle using the Kinematic Locomotion system
    void updateRotationKinematic(float dt)
    {
        rotation += angularVelocity * dt + 0.5f * angularAcceleration * Mathf.Pow(dt, 2);

        angularVelocity += angularAcceleration * dt;
    }



    // The following function attempts to show off what the physics engine can do
    // If you want to make changes how the particle moves, do it here
    void DemonstrateParticleMovement()
    {
        acceleration.x = -Mathf.Sin(Time.time);
        acceleration.y = -Mathf.Cos(Time.time);
    }



    void AddForce(Vector2 newForce)
    {
        // D'Almbert
        force += newForce;
    }



    void UpdateAcceleration()
    {
        // Convert force to acceleration
        acceleration = force * invMass;

        force.Set(0.0f,0.0f);
    }

    private void Update()
    {
        AddForce(ForceGenerator.GenerateForce_Gravtity(mass,GRAV_CONSTANT, Vector3.up));
        AddForce(ForceGenerator.GenerateForce_spring(position, new Vector2(0, 0), 5, 8.0f));
    }
}
