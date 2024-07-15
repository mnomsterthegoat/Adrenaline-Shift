using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Starter2 : MonoBehaviour
{
    public float MAX_VELOCITY = 100f; // CONSTANT
    public float TIME_TO_REACH = 20f; // CONSTANT
    private float currVelocity;
    private float currTime;
    private float a;
    private float moveSpeed;

    private Rigidbody playerRigidBody;
    // Start is called before the first frame update
    void Start()
    {

        currTime = 0f;     // Initialize the current time
        moveSpeed = 0f;    // Initialize the move speed

        playerRigidBody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        //currVelocity = playerRigidBody.velocity; // Initialize the current velocity

        if (currTime < TIME_TO_REACH) // Ensure we only accelerate for the specified time
        {
            currTime += Time.deltaTime;
            a = (MAX_VELOCITY - currVelocity) / (TIME_TO_REACH - currTime);
            currVelocity += a * Time.deltaTime;
            moveSpeed = currVelocity * Input.GetAxisRaw("Vertical");
        }
        else
        {
            // Once we reach the maximum velocity, maintain it
            moveSpeed = MAX_VELOCITY * Input.GetAxisRaw("Vertical"); ;
        }

        playerRigidBody.velocity = new Vector3(0, 0, moveSpeed);
    }
}