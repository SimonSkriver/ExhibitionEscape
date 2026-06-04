using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueUI : MonoBehaviour
{
    UI_Manager UI;
    Label dialogueText;
    List<Button> choices = new List<Button>();
     
    private void Awake() {
        UI = UI_Manager.Instance;
        
        dialogueText = UI.dialogueRoot.Q<Label>("DialogueText");
        choices = UI.dialogueRoot.Query<Button>().ToList();
        HideChoices();
    }

    #region Choices
    public void HideChoices() {
        DialogueManager.Instance.isShowingChoices = false;
        foreach (var choice in choices) {
            choice.RegisterCallback<ClickEvent>(OnChoiceSelected);
            choice.RegisterCallback<NavigationSubmitEvent>(OnChoiceSelected);
            choice.style.display = DisplayStyle.None;
        }
    }

    void ShowChoices(List<Choice> dialogueChoices) {
        foreach (Choice choice in dialogueChoices) {
            choices[choice.index].text = choice.text;
            choices[choice.index].style.display = DisplayStyle.Flex;
            DialogueManager.Instance.isShowingChoices = true;
        }
    }

    void OnChoiceSelected(ClickEvent evt) => ChoiceSelected((Button)evt.target);
    void OnChoiceSelected(NavigationSubmitEvent evt) => ChoiceSelected((Button)evt.target);
    void ChoiceSelected(Button btn) {
        DialogueManager.Instance.story.ChooseChoiceIndex(btn.tabIndex); // Connecting the button.tabIndex with the Ink choice index
        HideChoices();
        DialogueManager.Instance.ContinueOrExitStory();
    }
    #endregion

    #region DialogueLine
    public void ChangeDialogueUI(string dialogueLine, List<Choice> dialogueChoices) {
        // Write dialogue
        StartCoroutine(WriteDialogueLine(dialogueLine));

        // Enable and set choice info depending on ink information
        ShowChoices(dialogueChoices);
    }

    public void SkipDialogueWriting(string dialogueLine) => dialogueText.text = dialogueLine;

    IEnumerator WriteDialogueLine(string dialogueLine) {
        dialogueText.text = "";
        int loopsuntilSFX = 0;
        foreach (char c in dialogueLine.ToCharArray()) {
            if (DialogueManager.Instance.isWritingDialogue) {
                dialogueText.text += c.ToString();
                loopsuntilSFX++;
                if(loopsuntilSFX == 3)
                {
                    SFXManager.PlayEffect("SkeletonTalk");
                    loopsuntilSFX = 0;
                }
                yield return new WaitForSeconds(0.03f);
            }
        }

        DialogueManager.Instance.isWritingDialogue = false;
    }
    #endregion
}
