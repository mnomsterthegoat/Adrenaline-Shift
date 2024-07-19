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
    public GameObject Enemy;
    public GameObject EscMenu;
    // Start is called before the first frame update
    void Start()
    {
        Enemy.SetActive(false);
        redCar.SetActive(false);
        whiteCar.SetActive(false);
        greenCar.SetActive(false);
        EscMenu.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            EscMenu.SetActive(true);
            GameUI.SetActive(false);
        }
    }

    public void MapSelect()
    {
        SceneManager.LoadScene("MapSelect");
    }
    public void CloseMenu()
    {
        EscMenu.SetActive(false);
        Time.timeScale = 1;
        GameUI.SetActive(true);
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
        Enemy.SetActive(true);
    }
    public void Car2()
    {
        redCar.SetActive(false);
        whiteCar.SetActive(true);
        greenCar.SetActive(false);
        GameUI.SetActive(true);
        carCanvas.SetActive(false);
        Enemy.SetActive(true);
    }
    public void Car3()
    {
        redCar.SetActive(false);
        greenCar.SetActive(true);
        whiteCar.SetActive(false);
        GameUI.SetActive(true);
        carCanvas.SetActive(false);
        Enemy.SetActive(true);
    }

    public void Map1()
    {
        SceneManager.LoadScene("Map1");
    }
    public void Map2()
    {
        SceneManager.LoadScene("Map2");
    }
    public void Map3()
    {
        SceneManager.LoadScene("map3");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
