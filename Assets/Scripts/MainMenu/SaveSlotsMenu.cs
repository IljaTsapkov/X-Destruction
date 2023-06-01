using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{

    [SerializeField] private MainMenu mainMenu;

    [SerializeField] private Button backBtn;

    [SerializeField] private ConfirmationPopupMenu confirmationPopupMenu;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake() 
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();    
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        // disable all buttons
        DisableMenuButtons();

        // loading game
        if(isLoadingGame)
        {
            DataPersistence.instance.ChangeSelectedProfileID(saveSlot.GetProfileID());
            SaveGameAndLoadScene();
        }
        // new game, but the save slot has data
        else if (saveSlot.hasData)
        {
            confirmationPopupMenu.ActivateMenu(
                "Starting a New Game with this slot will overwrite currently saved data. Are you sure?",
                // function to execute if we select 'yes'
                () => {
                    DataPersistence.instance.ChangeSelectedProfileID(saveSlot.GetProfileID());
                    DataPersistence.instance.NewGame();
                    SaveGameAndLoadScene();
                },
                // function to execute if we select 'cancel'
                () => {
                    this.ActivateMenu(isLoadingGame);
                }
            );
        }
        // new game, and the save slot has no data
        else
        {
            DataPersistence.instance.ChangeSelectedProfileID(saveSlot.GetProfileID());
            DataPersistence.instance.NewGame();
            SaveGameAndLoadScene();
        }
    }
    
    private void SaveGameAndLoadScene()
    {
        // save the game anytime before loading a new scene
        DataPersistence.instance.SaveGame();

        Time.timeScale = 1f;

        // load the scene
        SceneManager.LoadSceneAsync(1);
    }

    public void onDeleteBtnClick(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confirmationPopupMenu.ActivateMenu(
            "Are you sure you want to delete this saved data?",
            // function to execute if we select 'yes'
            () => {
                DataPersistence.instance.DeleteProfileData(saveSlot.GetProfileID());
                ActivateMenu(isLoadingGame);
            },
            // function to execute if we select 'cancel'
            () => {
                ActivateMenu(isLoadingGame);
            }
        );
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        // set mode
        this.isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistence.instance.GetAllProfilesGameData();

        // ensure the back button is enabled when we activate the menu
        backBtn.interactable = true;

        // loop through each save slot in the UI and set the content appropriately
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else 
            {
                saveSlot.SetInteractable(true);
            }
        }   
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in  saveSlots) 
        {
            saveSlot.SetInteractable(false);
        }
        backBtn.interactable = false;
    }
}