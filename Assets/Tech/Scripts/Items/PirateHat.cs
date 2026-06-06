using UnityEngine;

public class PirateHat : MonoBehaviour, IInteractable {
    [SerializeField] Animator chestAnimator;
    [SerializeField] ParticleSystem particles;
    Transform hatAnchor, capHatAnchor;
    public bool hasHat { get; private set; }

    void Awake() {
        hatAnchor = GameObject.FindWithTag("HatAnchor").transform;
        capHatAnchor = GameObject.Find("Skeleton (Captain)/Skeleton/lowertorso/torsobone/skull/HatAnchor").transform;
        EventManager.Instance.CapHat += SetHatOnCaptain;
    }

    public void Interact() {
        hasHat = true;
        DialogueManager.Instance.ChangeInkVariable("hasTreasure", hasHat.ToString());

        chestAnimator.SetTrigger("CloseLid");

        transform.SetParent(hatAnchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.layer = 2;

        gameObject.layer = LayerMask.NameToLayer("Player");
        particles.Play();
        SFXManager.PlayEffect("ConfirmColor");
    }

    public bool ShowOutline() {
        if (!hasHat)
            return true;
        else
            return false;
    }

    public bool ShowInteract() {
        if (!hasHat)
            return true;
        else
            return false;
    }

    void SetHatOnCaptain() {
        transform.SetParent(capHatAnchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}