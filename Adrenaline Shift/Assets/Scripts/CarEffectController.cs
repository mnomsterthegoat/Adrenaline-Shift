using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffectController : MonoBehaviour
{
    public GameObject effectPrefab; // Reference to the effect prefab
    public float effectDuration = 1f; // Duration for which each effect instance lasts
    public float effectSpawnInterval = 0.1f; // Time interval between effect spawns

    void Start()
    {
        // Start the coroutine to continuously play the effect
        StartCoroutine(PlayEffectContinuously());
    }

    IEnumerator PlayEffectContinuously()
    {
        while (true) // Infinite loop to keep spawning effects
        {
            // Instantiate the effect prefab at the current position of the parent GameObject
            GameObject effectInstance = Instantiate(effectPrefab, transform.position, transform.rotation);

            // Make the effect instance follow the car
            StartCoroutine(FollowCar(effectInstance));

            // Destroy the effect instance after the specified duration
            Destroy(effectInstance, effectDuration);

            // Wait for the specified interval before spawning the next effect
            yield return new WaitForSeconds(effectSpawnInterval);
        }
    }

    IEnumerator FollowCar(GameObject effectInstance)
    {
        // Make the effect follow the car
        while (effectInstance != null)
        {
            effectInstance.transform.position = transform.position;
            effectInstance.transform.rotation = transform.rotation;
            yield return null; // Wait for the next frame
        }
    }
}
