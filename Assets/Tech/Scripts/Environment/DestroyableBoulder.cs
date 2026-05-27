using UnityEngine;

public class DestroyableBoulder : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (BoulderStrengthEffect.Instance == null)
            return;

        if (BoulderStrengthEffect.Instance.CanDestroyBoulders)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Eat some meat to destroy the boulder");
        }
    }

    public bool ShowOutline()
    {
        return true;
    }
}