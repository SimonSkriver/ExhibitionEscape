using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public DialogueEvents dialogueEvents;

    private void Awake()
    {
        if (Instance != null) { Debug.LogError("EventManager has multiple Instances"); }
        Instance = this;

        // initialize all events
        dialogueEvents = new DialogueEvents();
    }
}
