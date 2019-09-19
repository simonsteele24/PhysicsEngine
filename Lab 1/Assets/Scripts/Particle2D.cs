/*
Author: Simon Steele
Class: GPR-350-101
Assignment: Lab 2
Certification of Authenticity: We certify that this
assignment is entirely our own work.
*/

using UnityEngine;
using UnityEditor;

public class Particle2D : MonoBehaviour
{
    // Force Type Enum variables
    public ShapeType inertiaShapeType;
    public TorqueForce[] torqueForces;

    // Inertia - specific variables
    public Vector2 centreOfMass;
    public Vector2 boxDimensions;
    public float radius;
    public float innerRadius;
    public float outerRadius;
    public float length;


    // Vector2's
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    private Vector2 force;

    // Constants
    private Vector2 WORLD_UP = Vector2.up;
    const float BOX_INERTIA_CONSTANT = 1.0f / 12.0f;
    const float DISK_INERTIA_CONSTANT = 1.0f / 2.0f;

    // Floats
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;
    private float inertia;
    private float inverseInertia;
    private float torque = 0;
    [Range(0, Mathf.Infinity)] public float mass;
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

    private float Inertia
    {
        set
        {
            inertia = inertia > 0.0f ? inertia : 0.0f;
            inverseInertia = inertia > 0.0f ? 1.0f / inertia : 0.0f;
        }

        get
        {
            return inertia;
        }
    }


    private void Start()
    {
        Mass = mass;

        switch(inertiaShapeType)
        {
            case ShapeType.Square:
                inertia = BOX_INERTIA_CONSTANT * Mass * (Mathf.Pow(boxDimensions.x, 2) + Mathf.Pow(boxDimensions.y, 2));
                break;
            case ShapeType.Circle:
                inertia = DISK_INERTIA_CONSTANT * Mass * Mathf.Pow(radius, 2);
                break;
            case ShapeType.Disk:
                inertia = DISK_INERTIA_CONSTANT * Mass * (Mathf.Pow(innerRadius, 2) + Mathf.Pow(outerRadius, 2));
                break;
            case ShapeType.ThinRod:
                inertia = BOX_INERTIA_CONSTANT * Mass * length;
                break;
        }

        Inertia = inertia;

    }

    private void FixedUpdate()
    {
        updateRotationEulerExplicit(Time.fixedDeltaTime);
        updatePositionEulerExplicit(Time.fixedDeltaTime);

        UpdateAcceleration();
        UpdateAngularAcceleration();

        // Change position to the positional variables
        transform.position = position;

        // Change rotation to the rotational variables
        transform.eulerAngles = new Vector3(0, 0, rotation);
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



    public void AddForce(Vector2 newForce)
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

    void UpdateAngularAcceleration()
    {
        angularAcceleration = torque * inverseInertia;
        torque = 0;
    }


    public void ApplyTorque(Vector2 force, Vector2 location)
    {
        Vector3 cross = Vector3.Cross(force, location);
        torque += cross.z;
    }


    private void Update()
    {
        for (int i = 0; i < torqueForces.Length; i++)
        {
            ApplyTorque(torqueForces[i].force, torqueForces[i].position);
        }
    }
}




