using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestetFollow : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public Vector3 offset = new Vector3(0, 0, 6);
    private float targetRotationX;
    private float targetRotationY;
    private float targetRotationZ;


    void Start()
    {

    }
    void Update()
    {
        if (player1.activeInHierarchy)
        {
            targetRotationX = 0f;
            targetRotationY = player1.transform.rotation.y;
            targetRotationZ = 0f;
            transform.position = player1.transform.position + offset;
            transform.rotation = Quaternion.Euler(targetRotationX, targetRotationY, targetRotationZ);
        }
        if (player2.activeInHierarchy)
        {
            transform.position = player2.transform.position + offset;
        }
        if (player3.activeInHierarchy)
        {
            transform.position = player3.transform.position + offset;
        }
    }
}