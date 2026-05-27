using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    DialogueEvents dlgEvent;

    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJSON;
    public void SetInkJSON(TextAsset inkAsset) {
        inkJSON = inkAsset;
        SetStory();
    }
    public Story story { get; private set; }
    public bool isDialoguePlaying { get; private set; }

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        SetStory();

        dlgEvent = EventManager.Instance.dialogueEvents;
    }

    private void OnEnable() => dlgEvent.onEnterDialogue += EnterDialogue;
    private void OnDisable() => dlgEvent.onEnterDialogue -= EnterDialogue;

    void SetStory() => story = new Story(inkJSON.text);

    private void EnterDialogue(string knotName)
    {
        // Don't enter dialogue if we've already entered
        if (!isDialoguePlaying) {
            isDialoguePlaying = true;

            UI_Manager.Instance.ShowDialogueUI();

            // jump to the knot
            story.ChoosePathString(knotName);
        }

        ContinueOrExitStory();
    }

    public void ContinueOrExitStory()
    {
        if (story.canContinue) {
            string dialogueLine = story.Continue();
            EventManager.Instance.dialogueEvents.ChangeDialogueUI(dialogueLine, story.currentChoices);
        } else {
            ExitDialogue();
        }
    }

    private void ExitDialogue() {
        isDialoguePlaying = false;
        story.ResetState();
        UI_Manager.Instance.HideDialogueUI();
    }
}
