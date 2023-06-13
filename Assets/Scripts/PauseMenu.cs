using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool Paused = false; // to check if game is Paused
    public GameObject PauseMenuCanvas;
    public GameManagerScript gameManager;

    void Start()
    {
        Paused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if the gameOverUI is not active before allowing the pause menu to be opened
            if(!gameManager.gameOverUI.activeInHierarchy)
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
    }

    void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }



    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
        Cursor.visible = false;
    }

    public void MainMenuBtn()
    {
        PauseMenuCanvas.SetActive(false); // Disable PauseMenu visibility
        SceneManager.LoadSceneAsync(0); // Load MainMenu scene
    }

    public void QuitGame()
    {
        Debug.Log("Game quitted.");
        Application.Quit();
    }
}