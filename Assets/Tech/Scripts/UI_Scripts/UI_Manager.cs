using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour 
{
    public static UI_Manager Instance;
    

    VisualElement root;
    TemplateContainer pauseRoot, levelRoot, customizeRoot, dialogueRoot;

    Label dialogueText;
    List<Button> choices = new List<Button>();

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        pauseRoot = root.Q<TemplateContainer>("PauseMenu");
        levelRoot = root.Q<TemplateContainer>("LevelSelect");
        //customizeRoot = root.Q<TemplateContainer>("CustomizeMenu");

        // Dialogue
        dialogueRoot = root.Q<TemplateContainer>("DialogueMenu");
        dialogueText = dialogueRoot.Q<Label>("DialogueText");
        choices = dialogueRoot.Query<Button>().ToList();
        HideChoices();

        // Apply display style
        pauseRoot.style.display = DisplayStyle.None;
        levelRoot.style.display = DisplayStyle.None;
        dialogueRoot.style.display = DisplayStyle.None;
    }

    public void PauseMenuUI() {
        if (pauseRoot.style.display == DisplayStyle.None) {
            pauseRoot.style.display = DisplayStyle.Flex;
            InputManager.Instance.DisablePlayer();
        } else {
            pauseRoot.style.display = DisplayStyle.None;
            InputManager.Instance.EnablePlayer();
        }
    }

    public void LevelMenuUI() {
        if (levelRoot.style.display == DisplayStyle.None) {
            levelRoot.style.display = DisplayStyle.Flex;
            InputManager.Instance.DisablePlayer();
        } else {
            levelRoot.style.display = DisplayStyle.None;
            InputManager.Instance.EnablePlayer();
        }
    }


    #region Dialogue
    public void ShowDialogueUI() {
        dialogueRoot.style.display = DisplayStyle.Flex;
        InputManager.Instance.DisablePlayer();
    }

    public void HideDialogueUI() {
        dialogueRoot.style.display = DisplayStyle.None;
        InputManager.Instance.EnablePlayer();
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

    // Connect the method to the event system
    private void OnEnable() => EventManager.Instance.dialogueEvents.onChangeDialogueUI += ChangeDialogueUI;
    private void OnDisable() => EventManager.Instance.dialogueEvents.onChangeDialogueUI -= ChangeDialogueUI;
    #endregion

    #endregion
}