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

    private int currentChoiceIndex = -1;

    private void Awake()
    {
        story = new Story(inkJSON.text);
    }


    private void OnEnable()
    {
        EventManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        EventManager.Instance.dialogueEvents.onDisplayDialogue += UI_Manager.Instance.DisplayDialogue;
    }

    private void OnDisable()
    {
        EventManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        EventManager.Instance.dialogueEvents.onDisplayDialogue -= UI_Manager.Instance.DisplayDialogue;
    }


    private void EnterDialogue(string knotName)
    {
        UI_Manager.Instance.ShowDialogueUI();

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

        UI_Manager.Instance.HideDialogueUI();
    }

    private void OnChoiceSelected(ClickEvent evt)
    {
        Button button = (Button)evt.currentTarget;

        story.ChooseChoiceIndex(button.tabIndex);
        UI_Manager.Instance.HideChoices();
        ContinueOrExitStory();
        Debug.Log("You chose " + button.tabIndex);
    }



}
