using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour 
{
    public static UI_Manager Instance;

    public VisualElement root { get; private set; }
    public TemplateContainer pauseRoot { get; private set; }
    public TemplateContainer settingsRoot { get; private set; }
    public TemplateContainer levelRoot { get; private set; }
    public TemplateContainer customizeRoot { get; private set; }
    public TemplateContainer dialogueRoot { get; private set; }

    Label dialogueText;
    List<Button> choices = new List<Button>();

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        pauseRoot = root.Q<TemplateContainer>("PauseMenu");
        settingsRoot = root.Q<TemplateContainer>("SettingsMenu");
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

        //StartCoroutine(ScreenTransition());
    }

    public void ShowPauseMenuUI() => pauseRoot.style.display = DisplayStyle.Flex;
    public void HidePauseMenuUI() => pauseRoot.style.display = DisplayStyle.None;

    public void ShowLevelMenuUI() {
        levelRoot.style.display = DisplayStyle.Flex;
        InputManager.Instance.DisablePlayer();
    }
    public void HideLevelMenuUI() {
        levelRoot.style.display = DisplayStyle.None;
        InputManager.Instance.EnablePlayer();
    }

    public void ShowSettingsMenuUI() => settingsRoot.style.display = DisplayStyle.Flex;
    public void HideSettingsMenuUI() => settingsRoot.style.display = DisplayStyle.None;

    public IEnumerator ScreenTransition() {
        float alpha = 0;

        // Fade in
        while (alpha < 1 ) {
            yield return new WaitForSeconds(0.01f);
            alpha += 0.01f;
            root.Q<VisualElement>("TransitionScreen").style.backgroundColor = new Color(1, 1, 1, alpha);
            if (alpha > 1) { alpha = 1; }
            Debug.Log(alpha);
        }

        yield return new WaitUntil(() => alpha.Equals(1));
        yield return new WaitForSeconds(1f);

        // Fade out
        while (alpha > 0) {
            yield return new WaitForSeconds(0.01f);
            alpha -= 0.01f;
            root.Q<VisualElement>("TransitionScreen").style.backgroundColor = new Color(1, 1, 1, alpha);
            Debug.Log(alpha);
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