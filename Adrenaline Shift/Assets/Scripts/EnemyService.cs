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

    // Fields for the effect
    public GameObject effectPrefab; // Reference to the effect prefab
    public float effectDuration = 0.5f; // Duration for which each effect instance lasts
    public float effectSpawnInterval = 0.1f; // Time interval between effect spawns
    public Vector3 effectPositionOffset = new Vector3(0, 0, -2f); // Position offset of the effect relative to the car
    public Vector3 effectScale = new Vector3(1, 1, 1); // Scale of the effect
    public bool IsFollowCar = false; // Flag to determine if the effect follows the car

    // Fields for sound effects
    public AudioClip movingSound; // Sound effect for moving

    private AudioSource audioSource;
    private float currVelocity = 0f; // Current velocity
    private Rigidbody playerRigidBody;
    private Coroutine effectCoroutine;
    private bool isMovingSoundPlaying = false;

    public int checkpointCounter = 0;
    public GameObject[] checkpoints;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.drag = drag; // Set the drag on the Rigidbody
        playerRigidBody.useGravity = true; // Ensure gravity is applied

        audioSource = gameObject.AddComponent<AudioSource>();

        // Start the coroutine to continuously play the effects
        effectCoroutine = StartCoroutine(PlayEffectsContinuously());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 verticalInput = new Vector3(0, 0, 0);

        verticalInput = (checkpoints[checkpointCounter].transform.position - transform.position).normalized;

        if ((checkpoints[checkpointCounter].transform.position - transform.position).magnitude < 8)
        {
            checkpointCounter += 1;
            if (checkpointCounter > checkpoints.Length - 1)
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

            // Play moving sound effect if not already playing
            if (movingSound != null && !isMovingSoundPlaying)
            {
                PlaySound(movingSound);
                isMovingSoundPlaying = true;
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

            // Stop moving sound effect if playing
            if (movingSound != null && isMovingSoundPlaying)
            {
                audioSource.Stop();
                isMovingSoundPlaying = false;
            }
        }

        // Calculate movement direction
        Vector3 moveDirection = verticalInput * currVelocity;
        playerRigidBody.velocity = new Vector3(moveDirection.x, playerRigidBody.velocity.y, moveDirection.z);

        // Apply rotation for turning
        Vector3 lookPos = transform.position - checkpoints[checkpointCounter].transform.position; // Negate the direction vector
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, lookPos, MAX_VELOCITY * Time.deltaTime, 0.0f));

        // Optionally, log the current move speed and rotation for debugging purposes
        // Debug.Log(checkpointCounter);
    }

    IEnumerator PlayEffectsContinuously()
    {
        while (true) // Infinite loop to keep spawning effects
        {
            if (currVelocity > 0) // Only spawn the effect if the car is moving
            {
                // Instantiate and scale the effect
                InstantiateAndScaleEffect(effectPrefab, effectPositionOffset, effectScale, effectDuration);
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

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
