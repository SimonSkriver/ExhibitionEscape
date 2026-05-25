using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform eyes;
    [SerializeField] private Animator animator;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
        if (eyes == null)
        {
            eyes = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        }

        if (animator == null)
        {
            animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        }
    }

    public void Use()
    {
        InventorySlot selectedSlot = InventoryManager.Instance.GetSelectedSlot();

        if (selectedSlot == null || selectedSlot.IsEmpty)
            return;

        switch (selectedSlot.item.itemType)
        {
            case ItemType.Fruit:
                UseFruitItem(selectedSlot);
                break;

            case ItemType.Meat:
                UseMeatItem(selectedSlot);
                break;

            case ItemType.Axe:
                UseAxeItem(selectedSlot);
                break;
        }
    }

    private void UseFruitItem(InventorySlot slot)
    {
        FruitData fruitData = slot.item as FruitData;

        if (animator != null)
        {
            animator.SetTrigger("EAT");
        }

        FruitUseController.Instance.UseFruit(fruitData, transform);

        InventoryManager.Instance.RemoveSelectedItem();
    }

    private void UseMeatItem(InventorySlot slot)
    {
        MeatData meatData = slot.item as MeatData;

        if (animator != null)
        {
            animator.SetTrigger("EAT");
        }

        MeatUseController.Instance.UseMeat(meatData);

        if (meatData.IsLastStage(slot.currentStage))
        {
            InventoryManager.Instance.RemoveSelectedItem();
            MeatUseController.Instance.SpawnBone(meatData);
            
        }
        else
        {
            slot.currentStage++;

            EquipController.Instance.EquipSelectedSlot();

            if (PlayerHUD.Instance != null)
            {
                PlayerHUD.Instance.UpdateInventoryUI();
            }
        }
    }

    private void UseAxeItem(InventorySlot slot)
    {
        AxeData axeData = slot.item as AxeData;

        if (animator != null)
        {
            animator.SetTrigger("AXE_SWING");
        }

        Ray ray = new Ray(eyes.position, eyes.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, axeData.reach, ~playerLayer))
        {
            DestroyableLog log = hit.collider.GetComponentInParent<DestroyableLog>();

            if (log != null)
            {
                log.DestroyLog();
            }
        }
    }
}