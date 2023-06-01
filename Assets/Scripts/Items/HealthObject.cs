using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Object", menuName = "Inventory System/Items/Health")]
[Serializable]
public class HealthObject : ItemObject
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Health;
    }
}
