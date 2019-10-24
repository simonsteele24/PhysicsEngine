using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    // Inertia - specific variables
    public Vector3 centreOfMass;
    public Vector3 boxDimensions;

    // Vector2's
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    private Vector3 force;

    // Floats
    public Quaternion rotation;
    public Vector3 angularVelocity;
    public Vector3 angularAcceleration;


    private void Start()
    {
        position = transform.position;
    }



    private void FixedUpdate()
    {
        // Change position to the positional variables
        transform.position = position;

        // Update postion and velocity
        updateRotationKinematic(Time.fixedDeltaTime);
        updatePositionEulerExplicit(Time.fixedDeltaTime);

        transform.rotation = rotation;
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
        Quaternion newRot = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0);
        Quaternion temp = (rotation * newRot) * new Quaternion(0, 0, 0, (dt / 2.0f));
        rotation = new Quaternion(temp.x + rotation.x, temp.y + rotation.y, temp.z + rotation.z, temp.w + rotation.w).normalized;
    }



    // The following function calculates the new rotation of the
    // particle using the Kinematic Locomotion system
    void updateRotationKinematic(float dt)
    {

        Quaternion deltaTime = new Quaternion(0, 0, 0, (dt / 2.0f));
        Quaternion temp = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0) * deltaTime;
        Quaternion temp2 = new Quaternion(0, 0, 0, 0.5f) * new Quaternion(angularAcceleration.x,angularAcceleration.y,angularAcceleration.z,0) * deltaTime * deltaTime;
        Quaternion temp3 = new Quaternion(temp.x + temp2.x, temp.y + temp2.y, temp.z + temp2.z, temp.w + temp2.w) * rotation;
        rotation = new Quaternion(rotation.x + temp3.x, rotation.y + temp3.y, rotation.z + temp3.z, rotation.w + temp3.w).normalized;


        angularVelocity += angularAcceleration * dt;
    }



    public void AddForce(Vector3 newForce)
    {
        // D'Almbert
        force += newForce;
    }
}
