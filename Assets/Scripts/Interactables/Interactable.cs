using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;
    [SerializeField]
    public string promtMessage;
    public InventoryObject inventory;

    public virtual string OnLook()
    {
        return promtMessage;
    }

    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();

        GroundItem groundItem = GetComponent<GroundItem>();
        if (groundItem != null && inventory != null) // Check if inventory is not null
        {
            inventory.AddItem(new Item(groundItem.item), 1);
            Destroy(gameObject);
        }
        else
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        // Handle interaction logic here
    }
}
