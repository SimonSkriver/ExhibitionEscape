using UnityEngine;

public class DestroyableLog : MonoBehaviour, IInteractable
{
 public void Interact()
    {
        if(HasAxeEquipped()) DestroyLog();
    }
    
    public void DestroyLog()
    {
        Destroy(gameObject);
    }

    private bool HasAxeEquipped()
    {
        InventorySlot equippedSlot = InventoryManager.Instance.GetSelectedSlot();
        AxeData axeData = equippedSlot.item as AxeData;

        if (axeData == null)
        {
            Debug.LogWarning("Item is not AxeData");
            return false;
        }
        else return true;
    }
}