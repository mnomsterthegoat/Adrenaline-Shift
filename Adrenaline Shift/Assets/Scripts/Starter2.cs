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
    private float horizontalSpeed;
    public float smoothTime = 0.1f; // Time for smoothing

    private Vector3 velocity = Vector3.zero; // Velocity for smoothing

    private Rigidbody playerRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        currVelocity = 0f; // Initialize the current velocity
        currTime = 0f;     // Initialize the current time
        moveSpeed = 0f;    // Initialize the move speed

        playerRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (currTime < TIME_TO_REACH) // Ensure we only accelerate for the specified time
        {
            currTime += Time.deltaTime;
            a = (MAX_VELOCITY - currVelocity) / (TIME_TO_REACH - currTime);
            currVelocity += a * Time.deltaTime;
            moveSpeed = currVelocity * verticalInput;
        }
        else
        {
            // Once we reach the maximum velocity, maintain it
            moveSpeed = MAX_VELOCITY * verticalInput;
        }

        // Calculate horizontal speed
        horizontalSpeed = MAX_VELOCITY * horizontalInput;

        // Smoothly move the player
        Vector3 targetVelocity = new Vector3(horizontalSpeed, 0, moveSpeed);
        playerRigidBody.velocity = Vector3.SmoothDamp(playerRigidBody.velocity, targetVelocity, ref velocity, smoothTime);

        // Optionally, log the current move speed for debugging purposes
        Debug.Log("Move Speed: " + moveSpeed + ", Horizontal Speed: " + horizontalSpeed);
    }
}
