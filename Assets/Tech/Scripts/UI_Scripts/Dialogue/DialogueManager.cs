using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{   
    public static DialogueManager Instance;
    public bool isWritingDialogue;
    string dialogueLine;

    [Header("Ink Story")]
    private TextAsset inkJSON;
    public Story story { get; private set; }
    public bool isDialoguePlaying { get; private set; }
    public void SetInkJSON(TextAsset inkAsset) {
        inkJSON = inkAsset;
        story = new Story(inkJSON.text);

        // Print Ink ERROR message
        story.onError += (errorMessage, errorType) => {
        if (errorType == Ink.ErrorType.Warning)
            Debug.LogWarning(errorMessage);
        else
            Debug.LogError(errorMessage);
    };
    }




    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        // Default dialogue language
        if (LocalizationSettings.SelectedLocale.name.Contains("Danish")) {
            SetInkJSON(Resources.Load<TextAsset>("Dialogue/DA/DA_main"));
        } else {
            SetInkJSON(Resources.Load<TextAsset>("Dialogue/EN/EN_main"));
        }
    }

    public void EnterDialogue(string knotName)
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
    {   if (isWritingDialogue) {
            isWritingDialogue = false;
            UI_Manager.Instance.dialogueUI.SkipDialogueWriting(dialogueLine);
        } else {
            if (story.canContinue) {
                isWritingDialogue = true;
                dialogueLine = story.Continue();
                UI_Manager.Instance.dialogueUI.ChangeDialogueUI(dialogueLine, story.currentChoices);
            } else {
                ExitDialogue();
            }
        }
    }

    private void ExitDialogue() {
        isDialoguePlaying = false;
        story.ResetState();
        UI_Manager.Instance.HideDialogueUI();
    }
}
