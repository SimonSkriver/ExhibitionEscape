using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [Header("Item Data")]
    [SerializeField] private ItemData itemData;

    public void Interact()
    {
        if (itemData == null)
        {
            Debug.Log("No ItemData found");
            return;
        }

        InventoryManager.Instance.AddItem(itemData);

        Destroy(gameObject);
    }
}
