using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter2 : MonoBehaviour
{
    public float MAX_VELOCITY = 100f; // Maximum velocity
    public float TIME_TO_REACH = 5f; // Time to reach maximum velocity
    public float turnSpeed = 50f; // Speed at which the car turns
    public float decelerationRate = 0.5f; // Rate at which the car decelerates
    public float friction = 0.1f; // Friction factor to slow down the car gradually
    public float drag = 0.1f; // Drag factor to reduce velocity over time

    // Fields for the first effect
    public GameObject effectPrefab; // Reference to the first effect prefab
    public float effectDuration = 0.5f; // Duration for which each first effect instance lasts
    public float effectSpawnInterval = 0.1f; // Time interval between first effect spawns
    public Vector3 effectPositionOffset = new Vector3(0, 0, -2f); // Position offset of the first effect relative to the car
    public Vector3 effectScale = new Vector3(1, 1, 1); // Scale of the first effect

    // Fields for the second effect
    public GameObject effectPrefab2; // Reference to the second effect prefab
    public float effectDuration2 = 0.5f; // Duration for which each second effect instance lasts
    public float effectSpawnInterval2 = 0.1f; // Time interval between second effect spawns
    public Vector3 effectPositionOffset2 = new Vector3(0, 0, 0); // Position offset of the second effect relative to the car
    public Vector3 effectScale2 = new Vector3(1, 1, 1); // Scale of the second effect
    public bool IsFollowCar = false; // Flag to determine if the second effect follows the car

    // Reference to the particle system attached to the car
    public ParticleSystem moveParticleSystem;

    private float currVelocity = 0f; // Current velocity
    private Rigidbody playerRigidBody;
    private Coroutine effectCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.drag = drag; // Set the drag on the Rigidbody
        playerRigidBody.useGravity = true; // Ensure gravity is applied

        // Start the coroutine to continuously play the effects
        effectCoroutine = StartCoroutine(PlayEffectsContinuously());
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.S))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.W))
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
        if (verticalInput != 0) // Accelerate when there is vertical input
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
        Vector3 moveDirection = transform.forward * verticalInput * currVelocity;

        // Apply movement and maintain Y velocity (gravity)
        playerRigidBody.velocity = new Vector3(moveDirection.x, playerRigidBody.velocity.y, moveDirection.z);

        // Apply rotation for turning
        if (horizontalInput != 0 && verticalInput != 0) // Turn based on horizontal input
        {
            float turn = horizontalInput * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            playerRigidBody.MoveRotation(playerRigidBody.rotation * turnRotation);
        }

        // Control the particle system based on velocity
        if (currVelocity > 0)
        {
            if (!moveParticleSystem.isPlaying)
            {
                moveParticleSystem.Play();
            }
        }
        else
        {
            if (moveParticleSystem.isPlaying)
            {
                moveParticleSystem.Stop();
            }
        }

        // Optionally, log the current move speed and rotation for debugging purposes
        Debug.Log("Move Speed: " + currVelocity + ", Rotation: " + playerRigidBody.rotation.eulerAngles);
    }

    IEnumerator PlayEffectsContinuously()
    {
        while (true) // Infinite loop to keep spawning effects
        {
            if (currVelocity > 0) // Only spawn the first effect if the car is moving
            {
                // Instantiate and scale the first effect
                InstantiateAndScaleEffect(effectPrefab, effectPositionOffset, effectScale, effectDuration);
            }

            // Check for the second effect
            if (IsFollowCar)
            {
                if (currVelocity > 0) // Only spawn the second effect if the car is moving and IsFollowCar is true
                {
                    InstantiateAndScaleEffect(effectPrefab2, effectPositionOffset2, effectScale2, effectDuration2);
                }
            }
            else
            {
                // Continuously spawn the second effect regardless of the car's movement
                InstantiateAndScaleEffect(effectPrefab2, effectPositionOffset2, effectScale2, effectDuration2);
            }

            // Wait for the specified interval before spawning the next effect
            yield return new WaitForSeconds(effectSpawnInterval);
        }
    }

    private void InstantiateAndScaleEffect(GameObject effectPrefab, Vector3 positionOffset, Vector3 scale, float duration)
    {
        if (effectPrefab == null)
        {
            Debug.LogError("Effect prefab is not assigned!");
            return;
        }

        // Instantiate the effect prefab at the specified position and scale
        Vector3 position = transform.position + transform.TransformDirection(positionOffset);
        GameObject effectInstance = Instantiate(effectPrefab, position, transform.rotation);
        effectInstance.transform.localScale = Vector3.Scale(scale, transform.localScale); // Adjust the effect's scale based on the car's scale

        // Log the instantiation for debugging
        Debug.Log("Instantiated Effect: " + effectPrefab.name + " at position: " + effectInstance.transform.position + " with scale: " + effectInstance.transform.localScale);

        // Ensure the effect is on the correct layer and visible to the camera
        effectInstance.layer = gameObject.layer;

        // Activate the effect if it's inactive
        if (!effectInstance.activeInHierarchy)
        {
            effectInstance.SetActive(true);
        }

        // Ensure the particle system starts emitting
        ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
        else
        {
            Debug.LogError("No ParticleSystem component found on the effect prefab!");
        }

        // Destroy the effect instance after the specified duration
        Destroy(effectInstance, duration);
    }
}
