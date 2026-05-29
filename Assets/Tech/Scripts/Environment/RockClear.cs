using UnityEngine;

public class RockClear : MonoBehaviour, IInteractable
{
    private Rigidbody[] rocks;

    public void Interact()
    {
        SFXManager.PlayEffect("BoulderDestroy");
        rocks = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rocks)
        {
            rb.isKinematic = false;
        }
        Destroy(gameObject, 3f);
    }

    public bool ShowOutline()
    {
        return true;
    }
}
