using UnityEngine;

public class ItemUsage : MonoBehaviour
{
    public InventoryObject inventory;

    public DataPersistence dataPersistence;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseItemOnSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseItemOnSlot(1);
        }
    }


    public void UseItemOnSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventory.Container.Items.Length)
        {
            InventorySlot slot = inventory.Container.Items[slotIndex];
            if (slot.ID >= 0 && slot.item != null)
            {
                switch (slot.item.type)
                {
                    case ItemType.Health:
                        UseHealthPotion(slot);
                        break;
                    case ItemType.Armor:
                        UseArmorPotion(slot);
                        break;
                    default:
                        Debug.LogWarning("Cannot use this item.");
                        break;
                }
            }
        }
    }

    private void UseHealthPotion(InventorySlot slot)
    {
        float healAmount = 50f;
        PlayerHealth.Instance.RestoreHealth(healAmount);
        slot.AddAmount(-1);
        if (slot.amount <= 0)
        {
            inventory.RemoveItem(slot.item);
        }
        inventory.Save(dataPersistence.selectedProfileIDPublic);
        // inventory.Save();
    }

    private void UseArmorPotion(InventorySlot slot)
    {
        int armorValue = 10;
        PlayerHealth.Instance.RestoreArmor(armorValue);
        slot.AddAmount(-1);
        if (slot.amount <= 0)
        {
            inventory.RemoveItem(slot.item);
        }
        inventory.Save(dataPersistence.selectedProfileIDPublic);
    }
}
