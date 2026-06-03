using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;
    bool hasInteracted;

    public void Interact()
    {
        animator.SetTrigger("OpenLid");
        hasInteracted = true;
    }

    public bool ShowOutline()
    {
        if (hasInteracted) return false;
        else return true;
    }
}