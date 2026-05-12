using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class InventorySlotUI
{
    public VisualElement root;
    public Image itemIMG;
    public Label itemName;
    public Label itemCount;

    public InventorySlotUI(VisualElement slotRoot)
    {
        this.root = slotRoot;

        itemIMG = root.Q<Image>("item_image");
        itemName = root.Q<Label>("item_name");
        itemCount = root.Q<Label>("item_count");
    }
}