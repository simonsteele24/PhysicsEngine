/*
Author: Simon Steele
Class: GPR-350-101
Assignment: Lab 1
Certification of Authenticity: We certify that this
assignment is entirely our own work.
*/

using UnityEngine;
using UnityEditor;


public class ParticleManager : MonoBehaviour
{
    // Locomotion System Enum variables
    public LocomotionSystemType particleFormulaType;

    // Integers
    public int numOfParticles;

    // Vector2's
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;

    // Floats
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;
}





[CustomEditor(typeof(ParticleManager))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        // Set the script to the current instance
        var particleEventScript = target as ParticleManager;


        // Ask for all apropriate values of the particle
        
        // Locomotion Variables 
        particleEventScript.particleFormulaType = (LocomotionSystemType)EditorGUILayout.EnumPopup("Arc Type", particleEventScript.particleFormulaType);

        // Quantity Variables
        particleEventScript.numOfParticles = EditorGUILayout.IntField(new GUIContent("Number of particles", "The number of particles to be spawned"), particleEventScript.numOfParticles);

        // Positional-related Variables
        particleEventScript.position = EditorGUILayout.Vector2Field(new GUIContent("Position", "The starting position of the particle"), particleEventScript.position);
        particleEventScript.velocity = EditorGUILayout.Vector2Field(new GUIContent("Velocity", "The starting velocity of the particle"), particleEventScript.velocity);
        particleEventScript.acceleration = EditorGUILayout.Vector2Field(new GUIContent("Acceleration", "The starting acceleration of the particle"), particleEventScript.acceleration);

        // Rotational-related Variables
        particleEventScript.rotation = EditorGUILayout.FloatField(new GUIContent("Rotation", "The starting rotation of the particle"), particleEventScript.rotation);
        particleEventScript.angularVelocity = EditorGUILayout.FloatField(new GUIContent("Angular velocity", "The starting angular velocity of the particle"), particleEventScript.angularVelocity);
        particleEventScript.angularAcceleration = EditorGUILayout.FloatField(new GUIContent("Angular acceleration", "The starting angular acceleration of the particle"), particleEventScript.angularAcceleration);


        // Has the user pressed Spawn Particles button?
        if (GUILayout.Button("Spawn Set Particles"))
        {
            // If yes, spawn the particle(s)

            // Create an instance of the spawned particle
            GameObject particle = Resources.Load("Cube") as GameObject;

            // Set the position of the particle
            particle.transform.position = particleEventScript.position;

            // Set the rest of the appropriate Particle values
            particle.GetComponent<Particle2D>().LocomotionSystem = particleEventScript.particleFormulaType;
            particle.GetComponent<Particle2D>().position = particleEventScript.position;
            particle.GetComponent<Particle2D>().velocity = particleEventScript.velocity;
            particle.GetComponent<Particle2D>().acceleration = particleEventScript.acceleration;
            particle.GetComponent<Particle2D>().rotation = particleEventScript.rotation;
            particle.GetComponent<Particle2D>().angularVelocity = particleEventScript.angularVelocity;
            particle.GetComponent<Particle2D>().angularAcceleration = particleEventScript.angularAcceleration;

            // Instantiate X amount of particles, X being the number of particles inputted by the user
            for (int i = 0; i < particleEventScript.numOfParticles; i++)
            {
                Instantiate(particle, particle.transform.position, particle.transform.rotation);
            }
        }

    }
}