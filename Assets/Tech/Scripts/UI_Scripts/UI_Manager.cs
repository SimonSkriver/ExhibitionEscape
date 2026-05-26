using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Ink.Runtime;

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

    public void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices) {
        dialogueText.text = dialogueLine;

        //enable and set choice info depending on ink information
        foreach (Choice choice in dialogueChoices) {
            choices[choice.index].text = choice.text;
            choices[choice.index].style.display = DisplayStyle.Flex;
        }
    }

    public void HideChoices() {
        foreach (var choice in choices) {
            choice.RegisterCallback<ClickEvent>(OnChoiceSelected);
            choice.style.display = DisplayStyle.None;
        }
    }
    #endregion
}