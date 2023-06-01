using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Armor,
    Health,
    Default
}
[System.Serializable]
public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public float healthAmount; 
    public int armorValue;
    private void Awake()
    {
        type = ItemType.Health; // Set the default type to Health

    }



}
[System.Serializable]
public class Item
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public int Id;
    [SerializeField]
    public ItemObject itemObject;
    [SerializeField]
    public ItemType type;
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
        itemObject = item;
        type = item.type;
    }
}


