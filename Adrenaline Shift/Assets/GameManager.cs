using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    public void Car1()
    {
        SceneManager.LoadScene("MapSelect");
    }

    public void Map1()
    {
        SceneManager.LoadScene("devScene1234");
    }
    public void Map2()
    {
        SceneManager.LoadScene("Map2");
    }
    public void Map3()
    {
        SceneManager.LoadScene("Map3");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
