using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
   public GameObject gameOverUI;

    // public DataPersistence dataPersistence;

    // public void RetryBtn()
    // {
    //     gameOverUI.SetActive(false);
    //     Time.timeScale = 1f;
    //     Cursor.lockState = CursorLockMode.Confined;
    //     Cursor.visible = false;
    //     dataPersistence.LoadGame();
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }

    public void MainMenuBtn()
    {
        gameOverUI.SetActive(false); // Disable DeathMenu visibility
        SceneManager.LoadSceneAsync(0); // Load MainMenu scene
    }

    public void gameOver() {
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
