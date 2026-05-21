using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Settings")]
    [SerializeField] private int slotCount = 9;
    [SerializeField] private int stackLimit = 10;
    [SerializeField] public List<InventorySlot> slots;
    public int SlotCount => slotCount;
    public int StackLimit => stackLimit;

    [Header("Player / Item References")]
    public Transform itemPosition;

    [Header("Action References")]
    [SerializeField] InputAction slot1Action;
    [SerializeField] InputAction slot2Action;
    [SerializeField] InputAction slot3Action;
    [SerializeField] InputAction slot4Action;
    [SerializeField] InputAction slot5Action;
    [SerializeField] InputAction slot6Action;
    [SerializeField] InputAction slot7Action;
    [SerializeField] InputAction slot8Action;
    [SerializeField] InputAction slot9Action;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize inventory slots
        if (slots == null || slots.Count != slotCount)
        {
            slots = new List<InventorySlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                slots.Add(new InventorySlot());
            }
        }

        EnableInventoryActions();
    }

    public void AddItem(ItemData itemData)
    {
        foreach (var slot in slots) //First check if we already hold an item of this type and adds 1 to the stack.
        {
            if (slot.item == itemData && slot.count < stackLimit)
            {
                slot.count++;
                //InventoryUI.Instance.Refresh();
                return;
            }
        }

        foreach (var slot in slots) //Second put it in an empty slot if there is one
        {
            if (slot.IsEmpty)
            {
                slot.item = itemData;
                slot.count = 1;
                InventoryUI.Instance.Refresh();
                return;
            }
        }


        Debug.Log("Inventory full, cant add: " + itemData.itemName);
    }

    public void EquipItem(int slotIndex)
    {
        
    }
    public void UseItem(int slotIndex)
    {
        Debug.Log("Used item in slot " + slotIndex);

        if (slotIndex < 0 || slotIndex >= slotCount) return;

        var slot = slots[slotIndex];

        if (slot.IsEmpty) return;

        if (slot.item is FoodData food)
        {
            if(PlayerStats.Instance != null)
            {
                Debug.Log("Previous Player Saturation: " + PlayerStats.Instance.Saturation);

                PlayerStats.Instance.AddHealth(food.healAmount);

                if (PlayerStats.Instance.Saturation > 100) PlayerStats.Instance.Saturation = 100; // --------------- CHECK IT IN PlayerStats INSTEAD
                //if (PlayerStats.Instance.Health > 100) PlayerStats.Instance.Health = 100;

                Debug.Log("Player restored " + food.healAmount + " health.");
                
                BoostController.Instance.UseBoost(slot.item);
                slot.count--;
            }
        }

        if (slot.count <= 0)
        {
            slot.item = null;
            slot.count = 0;
        }
    }

    /*bool CheckIfMaxReached(int newStat) 
    {
        if (oldStat > 100) return true;
    }*/

    void EnableInventoryActions()
    {
        slot1Action = InputSystem.actions.FindAction("UseItem1");
        if (slot1Action != null)
        {
        slot1Action.Enable();
        slot1Action.performed += ctx => UseItem(0);
        }

        slot2Action = InputSystem.actions.FindAction("UseItem2");
        if (slot2Action != null)
        {
        slot2Action.Enable();
        slot2Action.performed += ctx => UseItem(1);
        }

        slot3Action = InputSystem.actions.FindAction("UseItem3");
        if (slot3Action != null)
        {
        slot3Action.Enable();
        slot3Action.performed += ctx => UseItem(2);
        }

        slot4Action = InputSystem.actions.FindAction("UseItem4");
        if (slot4Action != null)
        {
        slot4Action.Enable();
        slot4Action.performed += ctx => UseItem(3);
        }

        slot5Action = InputSystem.actions.FindAction("UseItem5");
        if (slot5Action != null)
        {
        slot5Action.Enable();
        slot5Action.performed += ctx => UseItem(4);
        }

        slot6Action = InputSystem.actions.FindAction("UseItem6");
        if (slot6Action != null)
        {
        slot6Action.Enable();
        slot6Action.performed += ctx => UseItem(5);
        }

        slot7Action = InputSystem.actions.FindAction("UseItem7");
        if (slot7Action != null)
        {
        slot7Action.Enable();
        slot7Action.performed += ctx => UseItem(6);
        }

        slot8Action = InputSystem.actions.FindAction("UseItem8");
        if (slot8Action != null)
        {
        slot8Action.Enable();
        slot8Action.performed += ctx => UseItem(7);
        }

        slot9Action = InputSystem.actions.FindAction("UseItem9");
        if (slot9Action != null)
        {
        slot9Action.Enable();
        slot9Action.performed += ctx => UseItem(8);
        }
    }
}
