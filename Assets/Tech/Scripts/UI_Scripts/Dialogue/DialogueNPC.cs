using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue (Optional)")]
    [SerializeField] private string dialogueKnotName;

    public void Interact()
    {
        // start dialogue if a knotName is defined
        if (dialogueKnotName != null)
        {
            EventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
}
