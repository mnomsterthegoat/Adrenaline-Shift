using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Starter2 : MonoBehaviour
{
    public float MAX_VELOCITY = 50f; // Maximum velocity
    public float TIME_TO_REACH = 5f; // Time to reach maximum velocity
    public float turnSpeed = 50f; // Speed at which the car turns
    public float decelerationRate = 0.5f; // Rate at which the car decelerates
    public float friction = 0.1f; // Friction factor to slow down the car gradually
    public float drag = 0.1f; // Drag factor to reduce velocity over time
    public float boosterFuel = 100f;

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

    // Fields for sound effects
    public AudioClip idleSound; // Sound effect for idling
    public AudioClip movingSound; // Sound effect for moving
    public AudioClip delayedMovingSound; // Sound effect for delayed moving
    public AudioClip maxSpeedSound; // Sound effect for reaching max speed

    private AudioSource audioSource;
    private float currVelocity = 0f; // Current velocity
    private Rigidbody playerRigidBody;
    private Coroutine effectCoroutine;
    private bool isMaxSpeedSoundPlaying = false;
    private bool isMovingSoundPlaying = false;
    private bool hasDelayedMovingSoundPlayed = false; // Ensure delayed moving sound plays only once

    private bool isSliding = false;
    private float slidingSpeed = 0f;
    private float slidingDecelerationRate = 1f; // Deceleration rate when sliding

    public int laps = 0;
    public TextMeshPro lapText;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.drag = drag; // Set the drag on the Rigidbody
        playerRigidBody.useGravity = true; // Ensure gravity is applied

        audioSource = gameObject.AddComponent<AudioSource>();

        // Start the coroutine to continuously play the effects
        effectCoroutine = StartCoroutine(PlayEffectsContinuously());

        // Start the idle sound effect
        if (idleSound != null)
        {
            audioSource.clip = idleSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        lapText.text = "LAPS: " + laps;
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.S))
        {
            verticalInput = 1f;
            isSliding = false; // Reset sliding when moving backward
        }
        else if (Input.GetKey(KeyCode.W))
        {
            verticalInput = -1f;
            isSliding = false; // Reset sliding when moving forward
        }

        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
            isSliding = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
            isSliding = true; 
        }

        // Calculate acceleration
        if (verticalInput != 0) // Accelerate when there is vertical input
        {
            if (currVelocity < MAX_VELOCITY)
            {
                currVelocity += (MAX_VELOCITY / TIME_TO_REACH) * Time.deltaTime;

                if (Input.GetKey(KeyCode.LeftShift) && boosterFuel > 0)
                {
                    currVelocity += 20f * Time.deltaTime;
                }
                else if (currVelocity > MAX_VELOCITY)
                {
                    currVelocity = MAX_VELOCITY;

                    // Play max speed sound effect if not already playing
                    if (maxSpeedSound != null && !isMaxSpeedSoundPlaying)
                    {
                        PlaySound(maxSpeedSound);
                        isMaxSpeedSoundPlaying = true;
                    }
                }
                if (currVelocity > 100)
                {
                    currVelocity = 100;
                }
            }
            slidingSpeed = currVelocity; // Update sliding speed
        }
        else if (isSliding) // Decelerate when sliding
        {
            if (slidingSpeed > 0)
            {
                slidingSpeed -= slidingDecelerationRate * Time.deltaTime;
                if (slidingSpeed < 0)
                {
                    slidingSpeed = 0;
                    isSliding = false;
                }
                currVelocity = slidingSpeed; // Update current velocity
            }
        }
        else // Decelerate when there is no vertical input and not sliding
        {
            if (currVelocity > 0)
            {
                currVelocity -= decelerationRate * Time.deltaTime * MAX_VELOCITY;
                if (currVelocity <= 0)
                {
                    currVelocity = playerRigidBody.velocity.z;
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && boosterFuel > 0)
        {
            boosterFuel -= 15f * Time.deltaTime;
        }

        if (boosterFuel < 0)
        {
            boosterFuel = 0;
        }

        //if (transform.position.y > 6)
        //{
         //   transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        //}
        //if (transform.position.y < -4)
        //{
        //    transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        //}

        // Calculate movement direction
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 moveDirection = transform.forward * -1 * currVelocity ;
            playerRigidBody.velocity = new Vector3(moveDirection.x, playerRigidBody.velocity.y, moveDirection.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 moveDirection = transform.forward * 1 * currVelocity;
            playerRigidBody.velocity = new Vector3(moveDirection.x, playerRigidBody.velocity.y, moveDirection.z);
        }

        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W))
        {
            currVelocity -= Time.deltaTime * 15f;
        }

        // Apply rotation for turning
        float turn = horizontalInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        playerRigidBody.MoveRotation(playerRigidBody.rotation * turnRotation);

        // Control the particle system based on velocity
        if (currVelocity > 0)
        {
            if (!moveParticleSystem.isPlaying)
            {
                moveParticleSystem.Play();
            }

            // Play moving sound effect if not already playing
            if (movingSound != null && !isMovingSoundPlaying)
            {
                PlaySound(movingSound);
                isMovingSoundPlaying = true;
            }

            // Trigger the delayed moving sound if speed is 1/10 of the total velocity and hasn't been played before
            if (delayedMovingSound != null && currVelocity >= MAX_VELOCITY / 10 && !hasDelayedMovingSoundPlayed)
            {
                PlaySoundOnce(delayedMovingSound);
                hasDelayedMovingSoundPlayed = true; // Ensure it plays only once
            }

            // Stop idle sound effect if playing
            if (idleSound != null && audioSource.clip == idleSound)
            {
                audioSource.Stop();
            }
        }
        else
        {
            if (moveParticleSystem.isPlaying)
            {
                moveParticleSystem.Stop();
            }

            // Play idle sound effect if not already playing
            if (idleSound != null && audioSource.clip != idleSound)
            {
                PlaySound(idleSound);
            }

            // Stop moving sound effect if playing
            if (movingSound != null && isMovingSoundPlaying)
            {
                isMovingSoundPlaying = false;
            }

            // Reset max speed sound playing flag
            isMaxSpeedSoundPlaying = false;
        }

        // Optionally, log the current move speed and rotation for debugging purposes
        Debug.Log("Move Speed: " + currVelocity + ", Rotation: " + playerRigidBody.rotation.eulerAngles);
    }

    IEnumerator PlayEffectsContinuously()
    {
        while (true) // Infinite loop to keep spawning effects
        {
            if (currVelocity > 0) // Only spawn the first effect if the car is moving forward
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

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void PlaySoundOnce(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void OnTriggerEnter(Collider other)
    {
        laps += 1;
        lapText.text = "LAPS: " + laps;
    }
}
