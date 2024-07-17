using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : MonoBehaviour
{
    public float MAX_VELOCITY = 50f; // Maximum velocity
    public float TIME_TO_REACH = 5f; // Time to reach maximum velocity
    public float turnSpeed = 50f; // Speed at which the car turns
    public float decelerationRate = 0.5f; // Rate at which the car decelerates
    public float friction = 0.1f; // Friction factor to slow down the car gradually
    public float drag = 0.1f; // Drag factor to reduce velocity over time

    // Reference to the particle system attached to the car
    public ParticleSystem moveParticleSystem;

    private float currVelocity = 0f; // Current velocity
    private Rigidbody playerRigidBody;

    public int checkpointCounter = 0;
    public GameObject[] checkpoints;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.drag = drag; // Set the drag on the Rigidbody
        playerRigidBody.useGravity = true; // Ensure gravity is applied
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 verticalInput = new Vector3(0,0,0);

        verticalInput = (checkpoints[checkpointCounter].transform.position - transform.position).normalized;

        if ((checkpoints[checkpointCounter].transform.position - transform.position).magnitude < 8)
        {
            checkpointCounter += 1;
            if (checkpointCounter > checkpoints.Length)
            {
                checkpointCounter = 0;
            }
        }

        // Calculate acceleration
       if (verticalInput.z != 0) // Accelerate when there is vertical input
        {
            if (currVelocity < MAX_VELOCITY)
            {
                currVelocity += (MAX_VELOCITY / TIME_TO_REACH) * Time.deltaTime;

                if (currVelocity > MAX_VELOCITY)
                {
                    currVelocity = MAX_VELOCITY;
                }
                if (currVelocity > 100)
                {
                    currVelocity = 100;
                }
            }
        }
        else // Decelerate when there is no vertical input
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
        Vector3 moveDirection = /*transform.forward * -*/verticalInput * currVelocity;
        playerRigidBody.velocity = new Vector3(moveDirection.x, playerRigidBody.velocity.y, moveDirection.z);

        // Apply rotation for turning
        Vector3 lookPos = checkpoints[checkpointCounter].transform.position - transform.position;
        lookPos.y = 0;
        //float turn =  -horizontalInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.LookRotation(-lookPos); //Quaternion.Euler(0f, turn, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, turnRotation, Time.deltaTime * 2);
        //playerRigidBody.MoveRotation(playerRigidBody.rotation * turnRotation);

        // Optionally, log the current move speed and rotation for debugging purposes
        Debug.Log(checkpointCounter);
    }
}
