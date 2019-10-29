using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    // Booleans
    public bool isUsingKinematicFormula = false;

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
        // Change position and rotation to the positional and rotational variables
        transform.position = position;
        transform.rotation = rotation;

        // Update postion and velocity

        // Should the program update rotation using the kinematic formula?
        if (isUsingKinematicFormula)
        {
            // If yes, then do so
            updateRotationKinematic(Time.fixedDeltaTime);
        }
        else
        {
            // If no, then use the Euler Explicit formula
            updateRotationEulerExplicit(Time.fixedDeltaTime);
        }

        updatePositionEulerExplicit(Time.fixedDeltaTime);
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





    public void AddForce(Vector3 newForce)
    {
        // D'Almbert
        force += newForce;
    }
}
