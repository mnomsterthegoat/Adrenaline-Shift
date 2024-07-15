using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPlayerScript : MonoBehaviour
{
    public float TIME_GOAL = 5;
    public float MAX_VELOCITY = 50;

    private Rigidbody myRigidBody;
    private float currTime = 0;
    private float acceleration = 0;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float currVelocity = myRigidBody.velocity.z;

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (currVelocity == 0)
            {
                currTime = 0;
                acceleration = (MAX_VELOCITY - currVelocity) / TIME_GOAL; // Initial acceleration calculation
            }
            else
            {
                currTime += Time.deltaTime;
                acceleration = (MAX_VELOCITY - currVelocity) / (TIME_GOAL - currTime);
            }

            float moveSpeed = MAX_VELOCITY + (acceleration * currTime);

            Debug.Log("Velocity: " + currVelocity + " | Acceleration: " + acceleration + " | Move Speed: " + moveSpeed);

            myRigidBody.velocity = new Vector3(0, 0, -moveSpeed * Input.GetAxisRaw("Vertical"));
        }
        else
        {
            // If no input, reset acceleration and move speed
            acceleration = 0;
            myRigidBody.velocity = Vector3.zero;
        }
    }
}
