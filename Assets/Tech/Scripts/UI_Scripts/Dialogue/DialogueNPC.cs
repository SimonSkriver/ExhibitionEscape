using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;
    Animator animator;

    public void Awake()
        {
            animator = GetComponent<Animator>();
        }
    public void Interact()
    {
        // start dialogue if a knotName is defined
        if (dialogueKnotName != "")
        {
            DialogueManager.Instance.EnterDialogue(dialogueKnotName);
        } else {
            Debug.LogWarning($"{gameObject.name} has no dialogueKnotName");
        }
        if(animator != null)
        {
            animator.SetTrigger("hasInteracted");
        }
    }
    

    public bool ShowOutline()
    {
        return true;
    }
}
