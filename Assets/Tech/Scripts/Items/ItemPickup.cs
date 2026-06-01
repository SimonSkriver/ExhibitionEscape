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

        bool added = InventoryManager.Instance.AddItem(itemData);
        SFXManager.PlayEffect("ItemPickup");

        if (!added) return;

        Animator anim = GameObject.FindWithTag("Player").GetComponent<Animator>();

        if (anim != null) 
        {
        anim.SetTrigger("PICK_UP");
        }


        Destroy(gameObject);
    }

    public bool ShowOutline()
    {
        return true;
    }
}
