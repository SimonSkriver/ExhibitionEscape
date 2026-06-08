using UnityEngine;

public class RockClear : MonoBehaviour, IInteractable
{
    private Rigidbody[] rocks;
    private Animator animator;

    public void Awake()
    {
        if (animator == null)
        {
            animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        }
    }
    public void Interact()
    {
        if (BoulderStrengthEffect.Instance == null)
            return;

        if (BoulderStrengthEffect.Instance.CanDestroyBoulders)
        {
            GetComponent<Collider>().enabled = false;
            SFXManager.PlayEffect("BoulderDestroy");
            animator.SetTrigger("ATTACK");
            rocks = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rocks)
            {
                rb.isKinematic = false;
            }
            Destroy(gameObject, 3f);
        }
        else
        {
            Debug.Log("Eat some meat to destroy the boulder");
            SFXManager.PlayEffect("Error");
        }
    }

    public bool ShowOutline()
    {
        return true;
    }

    public bool ShowInteract()
    {
        return true;
    }
}
