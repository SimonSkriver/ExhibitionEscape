using UnityEngine;

public class PirateHat : MonoBehaviour, IInteractable
{
    [SerializeField] Animator chestAnimator;
    [SerializeField] bool ready;
    Transform hatAnchor;
    public bool hasHat;

    void Awake()
    {
        hatAnchor = GameObject.FindWithTag("HatAnchor").transform;
    }

    public void Interact()
    {
        if (!ready) return;
        chestAnimator.SetTrigger("CloseLid");
        transform.SetParent(hatAnchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        hasHat = true;
    }

    public bool ShowOutline()
    {
        if (!hasHat) return true;
        else return false;
    }
}
