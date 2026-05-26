using System;
using System.Collections.Generic;
using Ink.Runtime;

public class DialogueEvents
{
    public event Action<string> onEnterDialogue;
    public void EnterDialogue(string knotName)
    {
        onEnterDialogue?.Invoke(knotName);
    }

    public event Action<string, List<Choice>> onChangeDialogueUI;
    public void ChangeDialogueUI(string dialogueLine, List<Choice> dialogueChoices)
    {
        onChangeDialogueUI?.Invoke(dialogueLine, dialogueChoices);
    }

    public event Action<int> onUpdateCoiceIndex;
    public void UpdateChoiceIndex(int choiceIndex)
    {
        onUpdateCoiceIndex?.Invoke(choiceIndex);
    }
}
