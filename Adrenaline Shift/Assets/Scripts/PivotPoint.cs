using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotPoint : MonoBehaviour
{
    public GameObject plr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = plr.transform.position;
    }
}
