using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;
    bool hasInteracted;

    public void Interact()
    {
        animator.SetTrigger("OpenLid");
        hasInteracted = true;
        SFXManager.PlayEffect("OpenChest");
    }

    public bool ShowOutline()
    {
        if (hasInteracted) return false;
        else return true;
    }

    public bool ShowInteract()
    {
        if (hasInteracted) return false;
        else return true;
    }
}