using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
[System.Serializable]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;

    public void AddItem(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == _item.Id)
            {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetEmptySlot(_item, _amount);
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.Id, _item, _amount);
                return Container.Items[i];
            }
        }
        // Add functionality for a full inventory here
        return null;
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    public void Save(string profileID)
    {
         Debug.Log("InventoryObject.Save called with profileID: " + profileID);
        string filePath = Path.Combine(Application.persistentDataPath, profileID, savePath);
        string json = JsonUtility.ToJson(Container);
        File.WriteAllText(filePath, json);
        Debug.Log("InventoryObject.Save finished");
    }

    public void Load(string profileID)
    {
        if (profileID == null)
        {
            return;
        }
        string filePath = Path.Combine(Application.persistentDataPath, profileID, savePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Inventory newContainer = JsonUtility.FromJson<Inventory>(json);
            for (int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
        }
    }

    public void Clear()
    {
        Container = new Inventory();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[2];
}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}