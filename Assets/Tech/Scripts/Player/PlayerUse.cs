using UnityEngine;
using System.Collections;

public class PlayerUse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform eyes;
    [SerializeField] private Animator animator;

    [Header("Layers")]
    [SerializeField] private LayerMask axeHitLayers;

    private bool shallEat = true;

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

        if (shallEat)
        {
            shallEat = false;
            if (animator != null)
            {
                animator.SetTrigger("EAT");
            }

            StartCoroutine(EatFruitAfterDelay(0.25f, fruitData, transform));
        }
    }

    private void UseMeatItem(InventorySlot slot)
    {
        MeatData meatData = slot.item as MeatData;

        if (shallEat)
        {
            shallEat = false;

            StartCoroutine(EatMeatAfterDelay(1f, meatData, slot));
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
            animator.SetTrigger("ATTACK");
            SFXManager.PlayEffect("AxeSwing");
        }

        Ray ray = new Ray(eyes.position, eyes.forward);

        Debug.DrawRay(eyes.position, eyes.forward * axeData.reach, Color.red, 2f);

        if (Physics.SphereCast(ray, 0.25f, out RaycastHit hit, axeData.reach, axeHitLayers)) //, QueryTriggerInteraction.Collide
        {
            Debug.Log("Axe hit: " + hit.collider.name);

            DestroyableLog log = hit.collider.GetComponentInParent<DestroyableLog>();
            BoarBehavior boar = hit.collider.GetComponentInParent<BoarBehavior>();

            if (log != null)
            {
                Debug.Log("Destroying log");
                log.DestroyLog();
            }

            if (boar != null)
            {
                Debug.Log("Hit Boar");
                boar.HitByAxe();
            }
        }
        else
        {
            Debug.Log("Axe hit nothing");
        }
    }

    private IEnumerator EatFruitAfterDelay(float seconds, FruitData fruit, Transform transform) 
    {
        InventoryManager.Instance.canScroll = false;
        yield return new WaitForSeconds(seconds);
        
        FruitUseController.Instance.UseFruit(fruit, transform);

        InventoryManager.Instance.RemoveSelectedItem();
        InventoryManager.Instance.canScroll = true;

        shallEat = true;
    }

    private IEnumerator EatMeatAfterDelay(float seconds, MeatData meat, InventorySlot slot) 
    { 
        if (MeatUseController.Instance.UseMeat(meat)) 
        {
            InventoryManager.Instance.canScroll = false;
            if (animator != null)
            {
                animator.SetTrigger("EAT_MEAT");
            }

            yield return new WaitForSeconds(seconds);

            if (meat.IsLastStage(slot.currentStage))
            {
                InventoryManager.Instance.RemoveSelectedItem();
                MeatUseController.Instance.SpawnBone(meat);
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
        shallEat = true;
        InventoryManager.Instance.canScroll = true;
    }
}