using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float MAX_VELOCITY = 100f; // Maximum velocity
    public float TIME_TO_REACH = 5f; // Time to reach maximum velocity
    public float handling = 20f; // Speed at which the car turns
    public float decelerationRate = 0.5f; // Rate at which the car decelerates
    private float currVelocity = 0f; // Current velocity
    private float moveSpeed; // Move speed
    private Rigidbody playerRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 0f; // Initialize the move speed
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

        // Calculate acceleration
        if (verticalInput != 0 || horizontalInput != 0) // Accelerate when there is input
        {
            if (currVelocity < MAX_VELOCITY)
            {
                currVelocity += (MAX_VELOCITY / TIME_TO_REACH) * Time.deltaTime;
                if (currVelocity > MAX_VELOCITY)
                {
                    currVelocity = MAX_VELOCITY;
                }
            }
        }
        else // Decelerate when there is no input
        {
            if (currVelocity > 0)
            {
                currVelocity -= decelerationRate * Time.deltaTime * MAX_VELOCITY;
                if (currVelocity < 0)
                {
                    currVelocity = 0;
                }
            }
        }

        // Calculate movement direction
        Vector3 moveDirection = transform.forward * verticalInput * currVelocity + transform.right * horizontalInput * currVelocity;

        // Apply movement
        playerRigidBody.velocity = moveDirection;

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.up * handling * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.up * -1 * handling * Time.deltaTime);
        }

        // Optionally, log the current move speed and rotation for debugging purposes
        Debug.Log("Move Speed: " + currVelocity + ", Rotation: " + playerRigidBody.rotation.eulerAngles);
    }
}
