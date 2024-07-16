using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ndLukeControl : MonoBehaviour
{
    [SerializeField]
    private float movespeed = 0f;
    [SerializeField]
    private float acceleration = 1f;
    private Rigidbody rb;
    public float handling = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            movespeed += acceleration * Time.deltaTime;
            rb.velocity = transform.forward * -1 * movespeed * Time.deltaTime; 

        }
        else
        {
            movespeed -= 500f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = transform.forward * movespeed * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.up * handling * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.up * -1 * handling * Time.deltaTime);
        }

        if(movespeed > 3000f)
        {
            movespeed = 3000f;
        }
        if (movespeed < 0f )
        {
            movespeed = 0f;
        }
    }
}
