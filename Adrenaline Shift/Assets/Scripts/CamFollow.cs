using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private GameObject player;
    public Vector3 offset = new Vector3(0, 20, 0);


    void Start()
    {

    }
    void LateUpdate()
    {
         player = GameObject.FindGameObjectWithTag("Player");
        
        
        transform.position = player.transform.position + offset;

    }
}