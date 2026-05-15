using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] private InventorySlotUI[] slotUIs;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        //Refresh();
    }

    private void OnEnable()
    {
        //Refresh();
    }

    public void Refresh()
    {
        var slots = InventoryManager.Instance.slots;

        for (int i = 0; i < slotUIs.Length; i++)
        {
            var ui = slotUIs[i];

            if (i >= slots.Count)
            {
                ClearSlot(ui);
                continue;
            }

            var slot = slots[i];

            if (slot.IsEmpty)
            {
                ClearSlot(ui);
            } 
            else
            {
                ui.itemIMG.sprite = slot.item.icon;
                ui.itemName.text = slot.item.itemName;

                if (slot.count > 1)
                {
                    ui.itemCount.text = slot.count.ToString();
                }
            }
        }
    }

    private void ClearSlot (InventorySlotUI ui)
    {
        ui.itemIMG.sprite = null;
        ui.itemName.text = "";
        ui.itemCount.text = "";
    }
}
