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

}
