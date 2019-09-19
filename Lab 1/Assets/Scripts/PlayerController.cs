using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Particle2D particle;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            particle.AddForce(new Vector2(speed, 0));
            particle.ApplyTorque(new Vector2(-speed, 0), new Vector2(0, transform.position.y + 1));
        }
        if (Input.GetKey(KeyCode.A))
        {
            particle.AddForce(new Vector2(-speed, 0));
            particle.ApplyTorque(new Vector2(speed, 0), new Vector2(0, transform.position.y + 1));
        }
    }
}
