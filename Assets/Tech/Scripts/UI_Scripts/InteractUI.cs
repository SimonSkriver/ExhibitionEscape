using UnityEngine;
using UnityEngine.UIElements;

public class InteractUI : MonoBehaviour
{
    public static InteractUI Instance;
    private VisualElement root;
    private Label interactLabel;

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        interactLabel = root.Q<Label>("Interact");

        HideInteractLabel();
    }

    public void ShowInteractLabel() => interactLabel.style.display = DisplayStyle.Flex;
    public void HideInteractLabel() => interactLabel.style.display = DisplayStyle.None;
}
