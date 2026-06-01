using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class PowerUpUI : MonoBehaviour
{
    public static PowerUpUI Instance;
    [SerializeField] private float iconShowDuration = 2f;
    [SerializeField] private float flickerAmount = 5;
    [SerializeField] private float flickerDelay = 0.15f;
    VisualElement root;
    Image imgJumpBoost, imgSpeedBoost, imgStrengthBoost;

    private Coroutine jumpCoroutine;
    private Coroutine speedCoroutine;
    private Coroutine strengthCoroutine;

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        imgJumpBoost = root.Q<Image>("JumpBoostIcon");
        imgSpeedBoost = root.Q<Image>("SpeedBoostIcon");
        imgStrengthBoost = root.Q<Image>("StrengthBoostIcon");

        HideJumpBoostIcon();
        HideSpeedBoostIcon();
        HideStrengthBoostIcon();
    }

    public void StartJumpIcon(float duration) 
    { 
        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
        }
        jumpCoroutine = StartCoroutine(JumpBoostNumerator(duration));
    }
    public void ShowJumpBoostIcon() => imgJumpBoost.style.display = DisplayStyle.Flex;
    public void HideJumpBoostIcon() => imgJumpBoost.style.display = DisplayStyle.None;

    public void StartSpeedIcon(float duration)
    { 
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
        }
        speedCoroutine = StartCoroutine(SpeedBoostNumerator(duration));
    }
    public void ShowSpeedBoostIcon() => imgSpeedBoost.style.display = DisplayStyle.Flex;
    public void HideSpeedBoostIcon() => imgSpeedBoost.style.display = DisplayStyle.None;

    public void StartStrengthIcon(float duration) 
    { 
        if (strengthCoroutine != null)
        {
            StopCoroutine(strengthCoroutine);
        }
        strengthCoroutine = StartCoroutine(StrengthBoostNumerator(duration));
    }
    public void ShowStrengthBoostIcon() => imgStrengthBoost.style.display = DisplayStyle.Flex;
    public void HideStrengthBoostIcon() => imgStrengthBoost.style.display = DisplayStyle.None;

    private IEnumerator JumpBoostNumerator(float duration)
    {
        ShowJumpBoostIcon();
        //yield return new WaitForSeconds(iconShowDuration);
        //HideJumpBoostIcon();

        yield return new WaitForSeconds(duration - 1);
       
        for (int i = 0; i < flickerAmount; i++)
        {
            ShowJumpBoostIcon();
            yield return new WaitForSeconds(flickerDelay);
            HideJumpBoostIcon();
            yield return new WaitForSeconds(flickerDelay);
        }
        HideJumpBoostIcon();
        jumpCoroutine = null;
    }

    private IEnumerator SpeedBoostNumerator(float duration)
    {
        ShowSpeedBoostIcon();
        //yield return new WaitForSeconds(iconShowDuration);
        //HideSpeedBoostIcon();

        yield return new WaitForSeconds(duration - 1);
       
        for (int i = 0; i < flickerAmount; i++)
        {
            ShowSpeedBoostIcon();
            yield return new WaitForSeconds(flickerDelay);
            HideSpeedBoostIcon();
            yield return new WaitForSeconds(flickerDelay);
        }
        HideSpeedBoostIcon();
        speedCoroutine = null;
    }

    private IEnumerator StrengthBoostNumerator(float duration)
    {
        ShowStrengthBoostIcon();
       // yield return new WaitForSeconds(iconShowDuration);
       // HideStrengthBoostIcon();

        yield return new WaitForSeconds(duration - 1);
       
        for (int i = 0; i < flickerAmount; i++)
        {
            ShowStrengthBoostIcon();
            yield return new WaitForSeconds(flickerDelay);
            HideStrengthBoostIcon();
            yield return new WaitForSeconds(flickerDelay);
        }
        HideStrengthBoostIcon();
        strengthCoroutine = null;
    }
}
