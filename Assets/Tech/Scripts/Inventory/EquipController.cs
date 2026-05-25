using UnityEngine;

public class EquipController : MonoBehaviour
{
    public static EquipController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform handTransform;

    private GameObject currentEquippedObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EquipSelectedSlot()
    {
        if (handTransform == null)
        {
            Debug.LogWarning("EquipController: No handTransform assigned.");
            return;
        }

        InventorySlot selectedSlot = InventoryManager.Instance.GetSelectedSlot();

        if (currentEquippedObject != null)
        {
            Destroy(currentEquippedObject);
        }

        if (selectedSlot == null)
        {
            Debug.Log("EquipController: No selected slot.");
            return;
        }

        if (selectedSlot.IsEmpty)
        {
            Debug.Log("EquipController: Selected slot is empty.");
            return;
        }

        GameObject prefabToEquip = GetEquippedPrefab(selectedSlot);

        if (prefabToEquip == null)
        {
            Debug.LogWarning("EquipController: No equipped prefab found for " + selectedSlot.item.itemName);
            return;
        }

        currentEquippedObject = Instantiate(prefabToEquip, handTransform.position, handTransform.rotation, handTransform);

        currentEquippedObject.transform.localPosition = Vector3.zero;
        currentEquippedObject.transform.localRotation = Quaternion.identity;

        Debug.Log("Equipped: " + selectedSlot.item.itemName);
    }

    private GameObject GetEquippedPrefab(InventorySlot slot)
    {
        if (slot == null || slot.item == null)
            return null;

        switch (slot.item.itemType)
        {
            case ItemType.Meat:
                MeatData meatData = slot.item as MeatData;

                if (meatData == null)
                {
                    Debug.LogWarning(slot.item.itemName + " is not MeatData.");
                    return slot.item.equippedPrefab;
                }

                return meatData.GetHeldPrefab(slot.currentStage);

            case ItemType.Fruit:

            case ItemType.Axe:
            
            default:
                return slot.item.equippedPrefab;
        }
    }
}