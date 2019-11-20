using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorldConnector : MonoBehaviour
{
    public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        //PhysicsNativePlugin.CreatePhysicsWorld();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PhysicsNativePlugin.GetCollision());
        //float x = position.x;
        //float y = position.y;
        //float z = position.z;

        //PhysicsNativePlugin.ChangePosition(ref x, ref y, ref z);

        //position = new Vector3(x, y, z);
        //Debug.Log(position);
    }
}
