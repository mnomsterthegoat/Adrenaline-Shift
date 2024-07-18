using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public Vector3 offset = new Vector3(0, 20, 0);


    void Start()
    {

    }
    void LateUpdate()
    {
        transform.position = player1.transform.position + offset;
        transform.position = player2.transform.position + offset;
        transform.position = player3.transform.position + offset;

    }
}