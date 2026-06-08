using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory")]
    [SerializeField] private int slotCount = 5;

    public List<InventorySlot> slots = new List<InventorySlot>();

    public int SelectedSlotIndex { get; private set; } = -1;
    public bool canScroll = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        slots.Clear();

        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.Log("Tried to add null item.");
            return false;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i].SetItem(itemData);

                if (SelectedSlotIndex == -1)
                {
                    SelectSlot(i);
                }

                PlayerHUD.Instance.UpdateInventoryUI();
                return true;
            }
        }

        Debug.Log("Inventory is full.");
        SFXManager.PlayEffect("Error");
        return false;
    }

    public void SelectSlot(int index)
    {
        if (!canScroll) return;

        if (index < 0 || index >= slots.Count)
            return;

        SelectedSlotIndex = index;

        EquipController.Instance.EquipSelectedSlot();
        PlayerHUD.Instance.UpdateInventoryUI();
    }

    public void ScrollSelect(float scrollValue)
    {
        if (!canScroll) return;
        
        if (slots.Count == 0)
            return;

        if (SelectedSlotIndex == -1)
        {
            SelectSlot(0);
            return;
        }

        int direction = scrollValue > 0 ? -1 : 1;
        int newIndex = SelectedSlotIndex + direction;

        if (newIndex < 0)
            newIndex = slots.Count - 1;

        if (newIndex >= slots.Count)
            newIndex = 0;

        SelectSlot(newIndex);
    }

    public InventorySlot GetSelectedSlot()
    {
        if (SelectedSlotIndex < 0 || SelectedSlotIndex >= slots.Count)
            return null;

        return slots[SelectedSlotIndex];
    }

    public void RemoveSelectedItem()
    {
        InventorySlot selectedSlot = GetSelectedSlot();

        if (selectedSlot == null)
            return;

        selectedSlot.Clear();

        EquipController.Instance.EquipSelectedSlot();
        PlayerHUD.Instance.UpdateInventoryUI();
    }
}