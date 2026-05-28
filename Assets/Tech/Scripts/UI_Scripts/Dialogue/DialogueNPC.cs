using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;

    public void Interact()
    {
        // start dialogue if a knotName is defined
        if (dialogueKnotName != "")
        {
            DialogueManager.Instance.EnterDialogue(dialogueKnotName);
        } else {
            Debug.LogWarning($"{gameObject.name} has no dialogueKnotName");
        }
    }

    public bool ShowOutline()
    {
        return true;
    }
}
