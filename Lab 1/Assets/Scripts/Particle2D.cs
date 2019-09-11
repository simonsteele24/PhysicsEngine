using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;

    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;

    public enum positionAndRotationFormulaType
    {
        EulerExplicit,
        Kinematic
    }

    public positionAndRotationFormulaType updateType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void updatePositionEulerExplicit(float deltaTime)
    {
        position += velocity * deltaTime;
        velocity += acceleration * deltaTime;
    }

    void updatePositionKinematic(float deltaTime)
    {
        position += velocity * deltaTime + 0.5f * acceleration * Mathf.Pow(deltaTime, 2);

        velocity += acceleration * deltaTime;
    }

    void updateRotationEulerExplicit(float deltaTime)
    {
        rotation += angularVelocity * deltaTime;
        angularVelocity += angularAcceleration * deltaTime;
    }

    void updateRotationKinematic(float deltaTime)
    {
        rotation += angularVelocity * deltaTime + 0.5f * angularAcceleration * Mathf.Pow(deltaTime, 2);

        angularVelocity += angularAcceleration * deltaTime;
    }

    private void FixedUpdate()
    {
        if (updateType == positionAndRotationFormulaType.EulerExplicit)
        {
            updateRotationEulerExplicit(Time.fixedDeltaTime);
            updatePositionEulerExplicit(Time.fixedDeltaTime);
        }
        else
        {
            updatePositionKinematic(Time.deltaTime);
            updateRotationKinematic(Time.deltaTime);
        }


        transform.position = position;
        transform.eulerAngles = new Vector3(0,0,rotation);


        acceleration.x = -Mathf.Sin(Time.time);
        acceleration.y = -Mathf.Cos(Time.time);
    }
}
