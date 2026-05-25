using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform eyes;
    [SerializeField] private Animator animator;

    [Header("Layers")]
    [SerializeField] private LayerMask axeHitLayers;

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

    if (axeData == null)
    {
        Debug.LogWarning("Item is not AxeData");
        return;
    }

    if (animator != null)
    {
        Debug.Log("Swung Axe");
        animator.SetTrigger("AXE_SWING");
    }

    Ray ray = new Ray(eyes.position, eyes.forward);

    Debug.DrawRay(eyes.position, eyes.forward * axeData.reach, Color.red, 2f);

    if (Physics.Raycast(ray, out RaycastHit hit, axeData.reach, axeHitLayers, QueryTriggerInteraction.Collide))
    {
        Debug.Log("Axe hit: " + hit.collider.name);

        DestroyableLog log = hit.collider.GetComponentInParent<DestroyableLog>();

        if (log != null)
        {
            Debug.Log("Destroying log");
            log.DestroyLog();
        }
    }
    else
    {
        Debug.Log("Axe hit nothing");
    }
}
}