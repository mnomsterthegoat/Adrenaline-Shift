using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCameraFollow : MonoBehaviour
{
    public GameObject parent;
    private Vector3 offset = new Vector3(1.5f, 3, 4);

    public float z = 0;
    public float x = 15;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(x, parent.transform.localEulerAngles.y, z);
    }
}
