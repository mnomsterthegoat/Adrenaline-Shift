using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0, 5, 6);


    void Start()
    {

    }
    void LateUpdate()
    {



        transform.position = player.transform.position + offset;

    }
}