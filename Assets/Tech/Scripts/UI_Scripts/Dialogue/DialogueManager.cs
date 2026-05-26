using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJSON;
    private Story story;
    
    private bool isDialoguePlaying = false;

    [Header("UI")]
    private VisualElement root;
    private Label dialogueText;
    private List<Button> choices = new List<Button>();

    private int currentChoiceIndex = -1;

    private void Awake()
    {
        story = new Story(inkJSON.text);

        root = GetComponent<UIDocument>().rootVisualElement;
        dialogueText = root.Q<Label>("DialogueText");
        choices = root.Query<Button>().ToList();

        HideChoices();

        root.style.display = DisplayStyle.None;
    }


    private void OnEnable()
    {
        EventManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        EventManager.Instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        EventManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        EventManager.Instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
    }


    private void EnterDialogue(string knotName)
    {
        InputManager.Instance.DisablePlayer();

        root.style.display = DisplayStyle.Flex;

        if (isDialoguePlaying)
        {
            isDialoguePlaying = true;

            // jump to the knot
            if (!knotName.Equals(""))
            {
                story.ChoosePathString(knotName);
            }
            else
            {
                Debug.LogWarning("Knot Name is empty");
            }    
        }
        ContinueOrExitStory();
    }


    private void ContinueOrExitStory()
    {
        if (story.canContinue)
        {
            string dialogueLine = story.Continue();
            EventManager.Instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);


            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            Debug.Log(dialogueLine);
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        Debug.Log("Exiting dialogue");

        isDialoguePlaying = false;

        story.ResetState();

        root.style.display = DisplayStyle.None;

        InputManager.Instance.EnablePlayer();
    }

    private void OnChoiceSelected(ClickEvent evt)
    {
        Button button = (Button)evt.currentTarget;

        story.ChooseChoiceIndex(button.tabIndex);
        HideChoices();
        ContinueOrExitStory();
        Debug.Log("You chose " + button.tabIndex);
    }

    private void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        dialogueText.text = dialogueLine;

        //enable and set choice info depending on ink information
        foreach (Choice choice in dialogueChoices)
        {
            choices[choice.index].text = choice.text;
            choices[choice.index].style.display = DisplayStyle.Flex;
        }
    }

    private void HideChoices()
    {
        foreach (var choice in choices)
        {
            choice.RegisterCallback<ClickEvent>(OnChoiceSelected);
            choice.style.display = DisplayStyle.None;
        }
    }
}
