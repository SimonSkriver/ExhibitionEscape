using UnityEngine;

public class DestroyableLog : MonoBehaviour, IInteractable
{
    [SerializeField] private TreeType treeType;

    [Header("Make list with fruit gameobjects on tree")]
    [SerializeField] private GameObject[] apples;
    [SerializeField] private GameObject[] bananas;
    [SerializeField] private GameObject[] coconuts;

    public void Interact()
    {
        /*if (HasAxeEquipped())
        {
            DestroyLog();
        }
        else SFXManager.PlayEffect("Error");*/
    }

    public void DestroyLog()
    {
        SFXManager.PlayEffect("TreeDestroy");
        switch (treeType)
        {
            case TreeType.PuzzleTree:
                Destroy(gameObject);
                break;

            case TreeType.AppleTree:
                DropFruits(apples);
                Destroy(gameObject);
                break;

            case TreeType.BananaTree:
                DropFruits(bananas);
                Destroy(gameObject);
                break;

            case TreeType.CoconutTree:
                DropFruits(coconuts);
                Destroy(gameObject);
                break;

            case TreeType.NormalTree:
                Destroy(gameObject);
                break;
        }
    }

    private void DropFruits(GameObject[] fruits)
    {
        foreach (GameObject fruit in fruits)
        {
            if (fruit == null) continue;

            fruit.transform.SetParent(null);

            Rigidbody rb = fruit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
        }
    }

    public bool ShowOutline()
    {
        Debug.Log("Showed because axe is equipped, is this true?: " + HasAxeEquipped());
        return HasAxeEquipped();
    }

    public bool ShowInteract()
    {
        return false;
    }

    private bool HasAxeEquipped()
    {
        InventorySlot equippedSlot = InventoryManager.Instance.GetSelectedSlot();

        if (equippedSlot == null || equippedSlot.item == null)
        {
            Debug.LogWarning("No item equipped");
            return false;
        }

        AxeData axeData = equippedSlot.item as AxeData;

        if (axeData == null)
        {
            Debug.LogWarning("Item is not AxeData");
            return false;
        }

        return true;
    }
}