using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    public static PlayerHUD Instance;

    VisualElement root;
    TemplateContainer HP;
    List<TemplateContainer> InvSlot;

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        HP = root.Q<TemplateContainer>("HP");

        InvSlot = root.Query<TemplateContainer>("InvSlot").ToList();
        foreach (var slot in InvSlot) {
            slot.Q<Label>("item_count").text = " ";
            slot.Q<Image>("item_icon").image = null;
            slot.Q<Label>("item_name").text = " ";
        }
    }

    public void UpdateHealthUI(int health) {
        HP.Q<Label>("txtHP").text = health.ToString();

        HP.Q<VisualElement>("HeartSlider").style.minHeight = Length.Percent(health);
        Debug.Log(Length.Percent(health));
    }

    public void UpdateInventoryUI()
    {
        if (InventoryManager.Instance == null)
            return;

        for (int i = 0; i < InvSlot.Count; i++)
        {
            bool slotExistsInInventory = i < InventoryManager.Instance.slots.Count;

            if (!slotExistsInInventory || InventoryManager.Instance.slots[i].IsEmpty)
            {
                UpdateInvSlotUI(i, 0, null, " ");
            }
            else
            {
                InventorySlot slot = InventoryManager.Instance.slots[i];
                ItemData item = slot.item;

                Texture iconTexture = GetSlotIconTexture(slot);

                UpdateInvSlotUI(i, 1, iconTexture, item.itemName);
            }

            bool isSelected = i == InventoryManager.Instance.SelectedSlotIndex;
            SetSelectedSlot(InvSlot[i], isSelected);
        }
    }

    private Texture GetSlotIconTexture(InventorySlot slot)
    {
        if (slot == null || slot.item == null)
            return null;

        Sprite iconSprite = slot.item.icon;

        if (slot.item.itemType == ItemType.Meat)
        {
            MeatData meatData = slot.item as MeatData;

            if (meatData != null)
            {
                iconSprite = meatData.GetInventoryIcon(slot.currentStage);
            }
        }

        if (iconSprite == null)
            return null;

        return iconSprite.texture;
    }

    public void UpdateInvSlotUI(int slotID, int itemCount, Texture itemIcon, string itemName) {

        // Only show itemCount if there's multiple
        if (itemCount > 1) {
            InvSlot[slotID].Q<Label>("item_count").text = itemCount.ToString();
        } else {
            InvSlot[slotID].Q<Label>("item_count").text = " ";
        }

        InvSlot[slotID].Q<Image>("item_icon").image = itemIcon;
        InvSlot[slotID].Q<Label>("item_name").text = itemName;
    }

    private void SetSelectedSlot(TemplateContainer slot, bool selected)
    {
        if (selected)
        {
            slot.style.borderTopWidth = 3;
            slot.style.borderBottomWidth = 3;
            slot.style.borderLeftWidth = 3;
            slot.style.borderRightWidth = 3;

            slot.style.borderTopColor = Color.yellow;
            slot.style.borderBottomColor = Color.yellow;
            slot.style.borderLeftColor = Color.yellow;
            slot.style.borderRightColor = Color.yellow;
        }
        else
        {
            slot.style.borderTopWidth = 0;
            slot.style.borderBottomWidth = 0;
            slot.style.borderLeftWidth = 0;
            slot.style.borderRightWidth = 0;

            slot.style.borderTopColor = Color.clear;
            slot.style.borderBottomColor = Color.clear;
            slot.style.borderLeftColor = Color.clear;
            slot.style.borderRightColor = Color.clear;
        }
    }
}
