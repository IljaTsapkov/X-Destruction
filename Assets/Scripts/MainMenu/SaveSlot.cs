using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{

    [SerializeField] private string profileID = "";

    [SerializeField] private GameObject noDataContent;

    [SerializeField] private GameObject hasDataContent;

    [SerializeField] private Button deleteBtn;

    private Button saveSlotBtn;

    public bool hasData { get; private set; } = false;
    
    private void Awake() 
    {
        saveSlotBtn = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if(data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            deleteBtn.gameObject.SetActive(false);
        }
        else 
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            deleteBtn.gameObject.SetActive(true);
        }
    }

    public string GetProfileID()
    {
        return this.profileID;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotBtn.interactable = interactable;
        deleteBtn.interactable = interactable;
    }
}
