using UnityEngine;
using UnityEngine.UIElements;

public class PowerUpUI : MonoBehaviour
{
    PowerUpUI Instance;
    VisualElement root;
    Image imgJumpBoost, imgSpeedBoost, imgStrengthBoost;

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        imgJumpBoost = root.Q<Image>("JumpBoostIcon");
        imgSpeedBoost = root.Q<Image>("SpeedBoostIcon");
        imgStrengthBoost = root.Q<Image>("StrengthBoostIcon");

        HideJumpBoostIcon();
        HideSpeedBoostIcon();
        HideStengthBoostIcon();
    }

    public void ShowJumpBoostIcon() => imgJumpBoost.style.display = DisplayStyle.Flex;
    public void HideJumpBoostIcon() => imgJumpBoost.style.display = DisplayStyle.None;
    public void ShowSpeedBoostIcon() => imgSpeedBoost.style.display = DisplayStyle.Flex;
    public void HideSpeedBoostIcon() => imgSpeedBoost.style.display = DisplayStyle.None;
    public void ShowStengthBoostIcon() => imgStrengthBoost.style.display = DisplayStyle.Flex;
    public void HideStengthBoostIcon() => imgStrengthBoost.style.display = DisplayStyle.None;
}
