using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool Paused = false; // to check if game is Paused
    public GameObject PauseMenuCanvas;

    void Start()
    {
        Paused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }



    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void MainMenuBtn()
    {
        Gun.shouldDestroy = false;
        PauseMenuCanvas.SetActive(false); // Disable PauseMenu visibility
        SceneManager.LoadSceneAsync(0); // Load MainMenu scene
    }
}
