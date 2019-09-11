using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum FormulaTypes
{
    EuclidianExplicit,
    Kinematic
};
public class ParticleManager : MonoBehaviour
{
    public Particle2D.positionAndRotationFormulaType particleFormulaType;

    public int numOfParticles;

    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;

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

        particleEventScript.particleFormulaType = (Particle2D.positionAndRotationFormulaType)EditorGUILayout.EnumPopup("Arc Type", particleEventScript.particleFormulaType);

        particleEventScript.numOfParticles = EditorGUILayout.IntField(new GUIContent("Number of particles", "The number of particles to be spawned"), particleEventScript.numOfParticles);

        particleEventScript.position = EditorGUILayout.Vector2Field(new GUIContent("Position", "The starting position of the particle"), particleEventScript.position);
        particleEventScript.velocity = EditorGUILayout.Vector2Field(new GUIContent("Velocity", "The starting velocity of the particle"), particleEventScript.velocity);
        particleEventScript.acceleration = EditorGUILayout.Vector2Field(new GUIContent("Acceleration", "The starting acceleration of the particle"), particleEventScript.acceleration);
        particleEventScript.rotation = EditorGUILayout.FloatField(new GUIContent("Rotation", "The starting rotation of the particle"), particleEventScript.rotation);
        particleEventScript.angularVelocity = EditorGUILayout.FloatField(new GUIContent("Angular velocity", "The starting angular velocity of the particle"), particleEventScript.angularVelocity);
        particleEventScript.angularAcceleration = EditorGUILayout.FloatField(new GUIContent("Angular acceleration", "The starting angular acceleration of the particle"), particleEventScript.angularAcceleration);

        if (GUILayout.Button("Spawn Set Particles"))
        {
            for (int i = 0; i < particleEventScript.numOfParticles; i++)
            {
                GameObject particle = Resources.Load("Cube") as GameObject;

                particle.transform.position = particleEventScript.position;

                particle.GetComponent<Particle2D>().updateType = particleEventScript.particleFormulaType;
                particle.GetComponent<Particle2D>().position = particleEventScript.position;
                particle.GetComponent<Particle2D>().velocity = particleEventScript.velocity;
                particle.GetComponent<Particle2D>().acceleration = particleEventScript.acceleration;
                particle.GetComponent<Particle2D>().rotation = particleEventScript.rotation;
                particle.GetComponent<Particle2D>().angularVelocity = particleEventScript.angularVelocity;
                particle.GetComponent<Particle2D>().angularAcceleration = particleEventScript.angularAcceleration;

                Instantiate(particle, particle.transform.position, particle.transform.rotation);

            }
        }

    }
}