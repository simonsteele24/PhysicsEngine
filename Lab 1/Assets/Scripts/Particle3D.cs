﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Particle3D : MonoBehaviour
{
    // Booleans
    public bool isUsingKinematicFormula = false;

    // Inertia - specific variables
    public Vector3 centreOfMass;
    private float inertia;
    private Matrix4x4 inertiaTensor;
    private float inverseInertia;

    // Vector3's
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    private Vector3 torque;
    public Matrix4x4 transformMatrix;
    public Matrix4x4 invTransformMatrix;

    // Dimensional Variables
    public float width;
    public float height;
    public float depth;
    public float radius;

    public int element;

    // Floats
    public Quaternion rotation;
    public ThirdDimensionalShapeType shape;
    public bool isHollow;
    public Vector3 angularVelocity;
    public Vector3 angularAcceleration;
    [Range(0, Mathf.Infinity)] public float mass;
    public float invMass;

    // Bonus - related variables
    public bool isGoingDownSlope = false;
    public GameObject slope;

    private float Mass
    {
        set
        {
            mass = mass > 0.0f ? mass : mass = 0.0f;
            invMass = mass > 0.0f ? invMass = 1.0f / mass : invMass = 0.0f;
        }

        get { return mass; }
    }

    private float Inertia
    {
        set
        {
            inertia = inertia > 0.0f ? inertia : inertia = 0.0f;
            inverseInertia = inertia > 0.0f ? inverseInertia = 1.0f / inertia : inverseInertia = 0.0f;
        }

        get { return inertia; }
    }




    private void Start()
    {

        if (!PhysicsNativePlugin.hasBeenEnabled)
        {
            PhysicsNativePlugin.CreatePhysicsWorld();
            PhysicsNativePlugin.hasBeenEnabled = true;
        }

        // Initialize values
        Mass = mass;
        position = transform.position;
        inertiaTensor = InertiaTensor.GetInertiaTensor(this, shape, isHollow);
        rotation = transform.rotation;

        element = PhysicsNativePlugin.AddParticle(mass, position.x, position.y, position.z);
    }





    private void FixedUpdate()
    {
        // Set the transformation matrices
        transformMatrix = Matrix4x4.TRS(transform.position, rotation, new Vector3(1,1,1));
        invTransformMatrix = transformMatrix.inverse;

        // Change position and rotation to the positional and rotational variables
        transform.position = position;

        PhysicsNativePlugin.AddForce(1, 0, 0, element);
        Vector3 newPos = position;
        PhysicsNativePlugin.UpdateParticle(ref newPos.x, ref newPos.y, ref newPos.z, Time.fixedDeltaTime, element);
        position = newPos;
    }





    // Returns the normal of a slope 
    Vector3 getSlopeNormal()
    {
        Vector3 rightwardVector = Vector3.right;



        return (Matrix4x4.TRS(slope.transform.position, slope.transform.rotation, new Vector3(1, 1, 1)) * (rightwardVector + slope.transform.position)).normalized;
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
        // Calculate out the whole formula for euler explicit:  q + wq dt/2

        // calculate w
        Quaternion newRot = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0);

        // wq dt/2
        Quaternion temp = (rotation * newRot) * new Quaternion(0, 0, 0, (dt / 2.0f));

        // (q) + (wq dt/2) and normalize the result
        rotation = new Quaternion(temp.x + rotation.x, temp.y + rotation.y, temp.z + rotation.z, temp.w + rotation.w).normalized;

        // Change Angular velocity as well
        angularVelocity += angularAcceleration * dt;
    }





    // This function add a specific point with a new force
    void AddForceAtPoint(Vector3 point, Vector3 newForce)
    {
        // Calculate the centre of mass and world position
        Vector3 worldCentreOfMass = transform.position;
        Vector3 pointWorldPosition = transformMatrix * point;

        // Get the cross product of the positional vector and the new force and then apply it
        torque = Vector3.Cross((pointWorldPosition - worldCentreOfMass), newForce);
        force += newForce;
    }





    void UpdateAcceleration()
    {
        // Convert force to acceleration
        acceleration = force * invMass;
        force.Set(0.0f, 0.0f,0.0f);
    }







    void UpdateAngularAcceleration()
    {
        // Update the angular acceleration and torque
        angularAcceleration = inertiaTensor * torque;
        torque.Set(0.0f, 0.0f, 0.0f);
    }





    // The following function calculates the new rotation of the
    // particle using the Kinematic Locomotion system
    void updateRotationKinematic(float dt)
    {
        // Calculate out the whole formula for kinematic: q + w * q * dt + 1/2 * angularaccel * q * dt^2

        // Calculate dt
        Quaternion deltaTime = new Quaternion(0, 0, 0, (dt / 2.0f));

        // w * dt
        Quaternion temp = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0) * deltaTime;

        // 1/2 w dt^2
        Quaternion temp2 = new Quaternion(0, 0, 0, 0.5f) * new Quaternion(angularAcceleration.x,angularAcceleration.y,angularAcceleration.z,0) * deltaTime * deltaTime;

        //((w dt) + (1/2 w dt^2))  * q
        Quaternion temp3 = new Quaternion(temp.x + temp2.x, temp.y + temp2.y, temp.z + temp2.z, temp.w + temp2.w) * rotation;

        //q + (((w dt) + (1/2 w dt^2))  * q) and normalize the result
        rotation = new Quaternion(rotation.x + temp3.x, rotation.y + temp3.y, rotation.z + temp3.z, rotation.w + temp3.w).normalized;

        // Change Angular velocity as well
        angularVelocity += angularAcceleration * dt;
    }
}
