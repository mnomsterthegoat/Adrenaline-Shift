using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private GameObject player;
    public Vector3 offset = new Vector3(0, 5, 6);


    void Start()
    {

    }
    void LateUpdate()
    {
        if (player = null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        transform.position = player.transform.position + offset;

    }
}