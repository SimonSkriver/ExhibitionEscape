using System.Collections;
using UnityEngine;

public class BoulderStrengthEffect : MonoBehaviour
{
    public static BoulderStrengthEffect Instance { get; private set; }

    public bool CanDestroyBoulders { get; private set; }

    private Coroutine strengthCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    public void StartBoulderStrength(float duration)
    {
        if (duration <= 0)
            return;

        if (strengthCoroutine != null)
        {
            StopCoroutine(strengthCoroutine);
        }

        strengthCoroutine = StartCoroutine(BoulderStrengthRoutine(duration));
    }

    private IEnumerator BoulderStrengthRoutine(float duration)
    {
        SFXManager.PlayEffect("StrengthBoost");
        PowerUpUI.Instance.StartStrengthIcon(duration);
        CanDestroyBoulders = true;

        yield return new WaitForSeconds(duration);

        CanDestroyBoulders = false;

        strengthCoroutine = null;
    }
}