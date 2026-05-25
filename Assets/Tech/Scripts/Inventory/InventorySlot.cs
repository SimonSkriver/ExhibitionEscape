[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int currentStage;

    public bool IsEmpty => item == null;

    public InventorySlot()
    {
        item = null;
        currentStage = 0;
    }

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        currentStage = 0;
    }

    public void Clear()
    {
        item = null;
        currentStage = 0;
    }
}