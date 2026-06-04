using UnityEngine;

public class PirateHat : MonoBehaviour, IInteractable
{
    Ink.Runtime.Object s;
    [SerializeField] Animator chestAnimator;
    [SerializeField] ParticleSystem particles;
    Transform hatAnchor;
    public bool hasHat { get; private set; }

    void Awake()
    {
        hatAnchor = GameObject.FindWithTag("HatAnchor").transform;
    }

    public void Interact()
    {
        hasHat = true;
        DialogueManager.Instance.ChanceInkVariable("hasTreasure", hasHat.ToString());

        chestAnimator.SetTrigger("CloseLid");

        transform.SetParent(hatAnchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.layer = 2;

        gameObject.layer = LayerMask.NameToLayer("Player");
        particles.Play();
        SFXManager.PlayEffect("ConfirmColor");
    }

    public bool ShowOutline()
    {
        if (!hasHat) return true;
        else return false;
    }
}