using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject redCar;
    public GameObject whiteCar;
    public GameObject greenCar;
    public GameObject GameUI;
    public GameObject carCanvas;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapSelect()
    {
        SceneManager.LoadScene("MapSelect");
    }

    public void ControlsSelect()
    {
        SceneManager.LoadScene("Controls");
    }

    public void Car1()
    {
        redCar.SetActive(true);
        whiteCar.SetActive(false);
        greenCar.SetActive(false);
        GameUI.SetActive(true);
        carCanvas.SetActive(false);
    }
    public void Car2()
    {
        redCar.SetActive(false);
        whiteCar.SetActive(true);
        greenCar.SetActive(false);
        GameUI.SetActive(true);
        carCanvas.SetActive(false);
    }
    public void Car3()
    {
        redCar.SetActive(false);
        greenCar.SetActive(false);
        whiteCar.SetActive(false);
        GameUI.SetActive(true);
        carCanvas.SetActive(false);
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
