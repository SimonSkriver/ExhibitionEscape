using UnityEngine;
public class DestroyableLog : MonoBehaviour, IInteractable
{
    [SerializeField] private TreeType treeType;

    [Header("Make list with fruit gameobjects on tree")]
    [SerializeField] private GameObject[] apples;
    [SerializeField] private GameObject[] coconuts;
 public void Interact()
    {
        if(HasAxeEquipped()) DestroyLog();
    }
    
    public void DestroyLog()
    {
        switch (treeType)
        {
            case TreeType.PuzzleTree:
                Destroy(gameObject);
                break;

            case TreeType.AppleTree:
                foreach (GameObject apple in apples)
                {
                    Rigidbody rb = apple.GetComponent<Rigidbody>();
                    rb.constraints = ~RigidbodyConstraints.FreezePositionY;
                }
                Destroy(gameObject);
                break;

            case TreeType.CoconutTree:
                foreach (GameObject coconut in coconuts)
                {
                    Rigidbody rb = coconut.GetComponent<Rigidbody>();
                    rb.constraints = ~RigidbodyConstraints.FreezePositionY;
                }
                Destroy(gameObject);
                break;
        }
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