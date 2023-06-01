using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [SerializeField] private Button newGameBtn;
    
    [SerializeField] private Button loadGameBtn;

    public void NewGame()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void LoadGame()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void QuitGame()
    {
        Debug.Log("Game quitted.");
        Application.Quit();
    }

    private void DisableMenuButtons()
    {
        newGameBtn.interactable = false;
        loadGameBtn.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
