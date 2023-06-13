using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    public InventoryObject inventory;

    public List<string> collectedUsables = new List<string>();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            Debug.Log(_item.Id);
            inventory.AddItem(_item, 1);
            collectedUsables.Add(other.gameObject.name);
            Destroy(other.gameObject);
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         inventory.Save();
    //     }
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         inventory.Load();
    //     }

    // }
    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[2];
    }

    public void LoadData(GameData data)
    {
        collectedUsables = data.collectedUsables;

        // Disable collected items in the scene
        foreach (string itemName in collectedUsables)
        {
            var item = GameObject.Find(itemName);
            if (item != null)
            {
                item.SetActive(false);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.collectedUsables = collectedUsables;
    }
}
