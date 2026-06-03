using UnityEngine;
using UnityEngine.UIElements;

public class InteractUI : MonoBehaviour
{
    public static InteractUI Instance;
    private VisualElement root;
    private Label interactLabel;
    private bool labelVisible;
    private Transform labelPosition;

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        interactLabel = root.Q<Label>("Interact");

        HideInteractLabel();
        labelVisible = false;
    }

    void Update()
    {
        if (labelVisible && labelPosition != null)
        {
            transform.position = labelPosition.position;
        }
    }

    public void ShowInteractLabel(Transform transform)
    {
        labelVisible = true;
        labelPosition = transform;
        interactLabel.style.display = DisplayStyle.Flex;
    }
    public void HideInteractLabel()
    {
        labelVisible = false;
        labelPosition = null;
        interactLabel.style.display = DisplayStyle.None;
    }

    public void UpdatePosition(Transform transform)
    {
        labelPosition = transform;
    }
}
