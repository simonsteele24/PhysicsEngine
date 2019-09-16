/*
Author: Simon Steele
Class: GPR-350-101
Assignment: Lab 2
Certification of Authenticity: We certify that this
assignment is entirely our own work.
*/

using UnityEngine;

public class ForceGenerator : MonoBehaviour
{
    // The following function adds a gravitational force
    public static Vector2 GenerateForce_Gravtity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        Vector2 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }

    // The following function adds a normalized force
    // from a gravitational force and a surface normal unt
    public static Vector2 GenerateForce_normal(Vector2 f_gravity, Vector2 surfaceNormal_unit)
    {
        Vector2 f_normal = Vector3.Project(f_gravity, surfaceNormal_unit);
        return f_normal;
    }


    // The following function generates a sliding force based on a gravitional
    // force and a normalized surface direction 
    public static Vector2 GenerateForce_sliding(Vector2 f_gravity, Vector2 f_normal)
    {
        Vector2 f_sliding = f_gravity + f_normal;
        return f_sliding;
    }


    // The following function generates a static frictional force based on a normalized 
    // force, an opposing forcing, and a static friction coefficient
    public static Vector2 GenerateForce_friction_static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient_static)
    {
        Vector2 f_friction_s = f_normal * frictionCoefficient_static;

        if (f_friction_s.magnitude < f_opposing.magnitude)
        {
            return -frictionCoefficient_static * f_normal;
        }
        else
        {
            return -f_opposing;
        }
    }


    // The following function generates a kinetic frictional force based on a normalized force,
    // a particle's velocity, and kinetic friction coefficient
    public static Vector2 GenerateForce_friction_kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        Vector2 f_friction_k = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized;
        return f_friction_k;
    }


    // The following force generates a drag force based on a particle's velocity, the fluid velocity
    //  of the thing that is applying the drag, the cross section of the particle, and a drag coefficient
    public static Vector2 GenerateForce_drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        Vector2 f_drag = (fluidDensity * particleVelocity * particleVelocity * objectArea_crossSection * objectDragCoefficient) / 2.0f;
        return f_drag;
    }



    // The following function generates a spring force on a particle based on the particle's position, the anchor's
    // position, the spring's rest position, and the stiffness of the spring
    public static Vector2 GenerateForce_spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        Vector2 dir = (anchorPosition - particlePosition).normalized;
        float f_spring = -springStiffnessCoefficient * ((anchorPosition - particlePosition).magnitude - springRestingLength);

        return dir * f_spring;
    }
}
