using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool CarSelectLoaded = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void CarSelect()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("CarSelect"));
        SceneManager.LoadScene("CarSelect");
    }

    public void ControlsSelect()
    {
        SceneManager.LoadScene("Controls");
    }
}
