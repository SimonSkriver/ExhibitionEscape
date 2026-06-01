using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class PowerUpUI : MonoBehaviour
{
    public static PowerUpUI Instance;
    [SerializeField] private float iconShowDuration = 2.5f;
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

    public void ShowJumpIcon() => StartCoroutine(ShowJumpBoostIcon());
    public void HideJumpBoostIcon() => imgJumpBoost.style.display = DisplayStyle.None;

    public void ShowSpeedIcon() => StartCoroutine(ShowSpeedBoostIcon());
    public void HideSpeedBoostIcon() => imgSpeedBoost.style.display = DisplayStyle.None;

    public void ShowStrengthIcon() => StartCoroutine(ShowStrengthBoostIcon());
    public void HideStengthBoostIcon() => imgStrengthBoost.style.display = DisplayStyle.None;

    private IEnumerator ShowJumpBoostIcon()
    {
        imgJumpBoost.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(iconShowDuration);
        HideJumpBoostIcon();
    }

    private IEnumerator ShowSpeedBoostIcon()
    {
        imgSpeedBoost.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(iconShowDuration);
        HideSpeedBoostIcon();
    }

    private IEnumerator ShowStrengthBoostIcon()
    {
        imgStrengthBoost.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(iconShowDuration);
        HideStengthBoostIcon();
    }
}
