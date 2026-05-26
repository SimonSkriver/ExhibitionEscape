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
            EventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        } else {
            Debug.LogWarning($"{gameObject.name} has no dialogueKnotName");
        }
    }
}
