using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
[Serializable]
public class ArmorObject : ItemObject
{
    public int restoreArmorValue;
    public void Awake()
    {
        type = ItemType.Armor;
    }
}
