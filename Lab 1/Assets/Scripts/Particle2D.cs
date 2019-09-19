﻿/*
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
    public ForceType[] forcesEnactedOnParticle;
    public ShapeType inertiaShapeType;

    // Force-specific variables
    public float gravitationalConstant;
    public Vector2 surfaceNormalUnit;
    public Vector2 normal;
    public Vector2 opposingForce;
    public float frictionCoefficient_static;
    public float frictionCoefficient_kinetic;
    public Vector2 fluidVelocity;
    public float fluidDensity;
    public float objectCrossection;
    public float dragCoefficient;
    public Vector2 anchorPosition;
    public float springRestingLength;
    public float springStiffness;
    public float waterHeight;
    public float maxDepth;
    public float volume;
    public float liquidDensity;

    // Vector2's
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;

    private Vector2 WORLD_UP = Vector2.up;

    // Floats
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;

    private float intertia;
    private float inverseInertia;

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

        switch(inertiaShapeType)
        {

        }

    }

    private void FixedUpdate()
    {
        updateRotationEulerExplicit(Time.fixedDeltaTime);
        updatePositionEulerExplicit(Time.fixedDeltaTime);

        UpdateAcceleration();

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
        // The following code goes through all forces that are supposed to be added
        // and determines their force type and acts appropriately
        for (int i = 0; i < forcesEnactedOnParticle.Length; i++)
        {
            switch (forcesEnactedOnParticle[i])
            {
                case ForceType.Gravity:
                    AddForce(ForceGenerator.GenerateForce_Gravtity(mass,gravitationalConstant,WORLD_UP));
                    break;
                case ForceType.Normal:
                    AddForce(ForceGenerator.GenerateForce_normal(new Vector2(0, gravitationalConstant), surfaceNormalUnit));
                    break;
                case ForceType.Sliding:
                    AddForce(ForceGenerator.GenerateForce_sliding(new Vector2(0, gravitationalConstant), normal));
                    break;
                case ForceType.Static_Friction:
                    AddForce(ForceGenerator.GenerateForce_friction_static(normal, opposingForce, frictionCoefficient_static));
                    break;
                case ForceType.Kinetic_Friction:
                    AddForce(ForceGenerator.GenerateForce_friction_kinetic(normal, velocity, frictionCoefficient_kinetic));
                    break;
                case ForceType.Drag:
                    AddForce(ForceGenerator.GenerateForce_drag(velocity, fluidVelocity, fluidDensity, objectCrossection, dragCoefficient));
                    break;
                case ForceType.Spring:
                    AddForce(ForceGenerator.GenerateForce_spring(position, anchorPosition, springRestingLength, springStiffness));
                    break;
                case ForceType.Buoyency:
                    AddForce(ForceGenerator.GenerateForce_buoyancy(position, waterHeight, maxDepth, volume, liquidDensity));
                    break;
                default:
                    break;
            }
        }
    }
}




