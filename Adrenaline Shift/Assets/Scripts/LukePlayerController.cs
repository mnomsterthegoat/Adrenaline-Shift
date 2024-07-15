using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LukePlayerController : MonoBehaviour
{
    public float MAX_VELOCITY = 100f; // CONSTANT
    public float TIME_TO_REACH = 20f; // CONSTANT
    private float currentVelocity;
    private float currentTime;
    private float acceleration;
    private float moveSpeed;
    public float handling = 0f;
    public GameObject player;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        currentTime = 0f;     // Initialize the current time
        moveSpeed = 0f;    // Initialize the move speed

        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        //currVelocity = playerRigidBody.velocity; // Initialize the current velocity

        if (currentTime < TIME_TO_REACH) // Ensure we only accelerate for the specified time
        {
            currentTime += Time.deltaTime;
            acceleration = (MAX_VELOCITY - currentVelocity) / (TIME_TO_REACH - currentTime);
            currentVelocity += acceleration * Time.deltaTime;
            moveSpeed = currentVelocity * Input.GetAxis("Vertical");
        }
        else
        {
            // Once we reach the maximum velocity, maintain it
            moveSpeed = MAX_VELOCITY * Input.GetAxisRaw("Vertical"); ;
        }
        rb.AddForce(player.transform.forward * -1 * moveSpeed * Time.deltaTime);
        
        Debug.Log(currentVelocity);
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.up * handling * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.up * -1 * handling * Time.deltaTime);
        }
    }
}
