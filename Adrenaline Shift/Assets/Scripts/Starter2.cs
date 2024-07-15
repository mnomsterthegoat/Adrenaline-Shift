using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter2 : MonoBehaviour
{
    public float MAX_VELOCITY = 100f; // CONSTANT
    public float TIME_TO_REACH = 20f; // CONSTANT
    public float turnSpeed = 100f; // Speed at which the car turns
    private float currVelocity;
    private float currTime;
    private float a;
    private float moveSpeed;

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
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        if (currTime < TIME_TO_REACH) // Ensure we only accelerate for the specified time
        {
            currTime += Time.deltaTime;
            a = (MAX_VELOCITY - currVelocity) / (TIME_TO_REACH - currTime);
            currVelocity += a * Time.deltaTime;
            moveSpeed = currVelocity;
        }
        else
        {
            // Once we reach the maximum velocity, maintain it
            moveSpeed = MAX_VELOCITY;
        }

        // Calculate movement direction
        Vector3 moveDirection = transform.forward * verticalInput * moveSpeed;

        // Apply movement
        playerRigidBody.velocity = moveDirection;

        // Apply rotation for turning
        if (horizontalInput != 0) // Only turn if there is horizontal input
        {
            float turn = horizontalInput * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            playerRigidBody.MoveRotation(playerRigidBody.rotation * turnRotation);
        }

        // Optionally, log the current move speed and rotation for debugging purposes
        Debug.Log("Move Speed: " + moveSpeed + ", Rotation: " + playerRigidBody.rotation.eulerAngles);
    }
}
