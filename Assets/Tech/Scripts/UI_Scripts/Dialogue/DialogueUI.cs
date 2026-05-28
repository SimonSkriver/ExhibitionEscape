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
        foreach (var choice in choices) {
            choice.RegisterCallback<ClickEvent>(OnChoiceSelected);
            choice.style.display = DisplayStyle.None;
        }
    }

    void ShowChoices(List<Choice> dialogueChoices) {
        foreach (Choice choice in dialogueChoices) {
            choices[choice.index].text = choice.text;
            choices[choice.index].style.display = DisplayStyle.Flex;
        }
    }

    void OnChoiceSelected(ClickEvent evt) {
        Button button = (Button)evt.currentTarget;
        DialogueManager.Instance.story.ChooseChoiceIndex(button.tabIndex); // Connecting the button.tabIndex with the Ink choice index
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

    IEnumerator WriteDialogueLine(string dialogueLine) {
        dialogueText.text = "";

        foreach (char c in dialogueLine.ToCharArray()) {
            dialogueText.text += c.ToString();
            yield return new WaitForSeconds(0.03f);
        }
    }
    #endregion
}
