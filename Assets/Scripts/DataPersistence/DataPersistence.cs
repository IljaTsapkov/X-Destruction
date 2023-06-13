using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistence : MonoBehaviour
{

    [SerializeField] private bool disableDataPersistence = false;

    [SerializeField] private bool initializeDataIfNull = false;

    [SerializeField] private bool overrideSelectedProfileID = false;

    [SerializeField] private string testSelectedProfileID = "test";

    
    [SerializeField] private string fileName;

    [SerializeField] private Vector3 startPos;


    [SerializeField] private InventoryObject inventoryObject;

    private GameData gameData;
    public GameData gameDataPublic
    {
        get { return gameData; }
    }

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileID = "";
    public string selectedProfileIDPublic
    {
        get { return selectedProfileID; }
    }

    public static DataPersistence instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the new one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if(disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is now disabled.");
        }
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        this.selectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileID();

        InitializeSelectedProfileID();
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame(); 
    }

    public void ChangeSelectedProfileID(string newProfileID)
    {
        Debug.Log("ChangeSelectedProfileID called with newProfileID: " + newProfileID);
        // update the profile to use for saving and loading
        this.selectedProfileID= newProfileID;
        // load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }

    public void DeleteProfileData(string profileID)
    {
            if (profileID == null)
            {
                return;
            }
        // delete the data for this profile id
        dataHandler.Delete(profileID);
        // initialize the selected profile id
        InitializeSelectedProfileID();
        // reload the game so that our data matches the newly selected profile id
        LoadGame();
    }

    private void InitializeSelectedProfileID()
    {
        this.selectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileID();
        Debug.Log("InitializeSelectedProfileID called with selectedProfileID: " + selectedProfileID);
        if(overrideSelectedProfileID)
        {
            this.selectedProfileID = testSelectedProfileID;
            Debug.LogWarning("Overrode selected profile ID with ID: " + testSelectedProfileID);
            NewGame();
            SaveGame();
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        inventoryObject.Clear();
    }

    public void LoadGame()
    {
        // return right away if data persistence is disabled
        if(disableDataPersistence)
        {
            return;
        }

        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load(selectedProfileID);

        // start a new game if the data is null and initialize data for debugging purposes
        if (this.gameData == null && initializeDataIfNull)  
        {
            NewGame();
        }

        // if no data can be loaded, don't continue
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A New Game must be started before any data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        // load inventory 
        inventoryObject.Load(selectedProfileID);
    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if(disableDataPersistence)
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game must be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // timestamp the data so we know when it was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();
    
        // save that data to a file using the data handler
        dataHandler.Save(gameData, selectedProfileID);
        Debug.Log("Finished calling dataHandler.Save");

        inventoryObject.Save(selectedProfileID);
    }

    // private void OnApplicationQuit() 
    // {
    //     SaveGame(); 
    // }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).
            OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    // TODO: Add continue button
    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}