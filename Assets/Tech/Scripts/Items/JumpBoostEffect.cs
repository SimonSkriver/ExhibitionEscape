using System.Collections;
using UnityEngine;

public class JumpBoostEffect : MonoBehaviour
{
    private Coroutine boostCoroutine;
    private float currentBoostAmount;

    public void StartBoost(float amount, float duration)
    {
        if (amount <= 0 || duration <= 0)
            return;

        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);

            if (amount > currentBoostAmount)
            {
                float difference = amount - currentBoostAmount;
                PlayerStats.Instance.JumpPower += difference;
                PlayerStats.Instance.canTakeFallDamage = false;
                currentBoostAmount = amount;
            }
        }
        else
        {
            currentBoostAmount = amount;
            PlayerStats.Instance.JumpPower += currentBoostAmount;
            PlayerStats.Instance.canTakeFallDamage = false;
        }

        boostCoroutine = StartCoroutine(BoostRoutine(duration));
    }

    private IEnumerator BoostRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        PlayerStats.Instance.JumpPower -= currentBoostAmount;
        PlayerStats.Instance.canTakeFallDamage = true;
        currentBoostAmount = 0;
        boostCoroutine = null;
    }
}