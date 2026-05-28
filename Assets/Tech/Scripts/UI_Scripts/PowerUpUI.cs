using UnityEngine;
using UnityEngine.UIElements;

public class PowerUpUI : MonoBehaviour
{
    PowerUpUI Instance;
    VisualElement root;
    Image imgJumpBoost;

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        imgJumpBoost = root.Q<Image>("JumpBoostIcon");
        
        HideJumpBoostIcon();
    }

    public void ShowJumpBoostIcon() => imgJumpBoost.style.display = DisplayStyle.Flex;
    public void HideJumpBoostIcon() => imgJumpBoost.style.display = DisplayStyle.None;
}
