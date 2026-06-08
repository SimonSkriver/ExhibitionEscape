using System.Collections;
using UnityEngine;

public class MovementBoostEffect : MonoBehaviour
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
                PlayerStats.Instance.MovementSpeed += difference;
                currentBoostAmount = amount;
            }
        }
        else
        {
            currentBoostAmount = amount;
            PlayerStats.Instance.MovementSpeed += currentBoostAmount;
        }

        boostCoroutine = StartCoroutine(BoostRoutine(duration));
    }

    private IEnumerator BoostRoutine(float duration)
    {
        PlayerController.canShowSpeedLines = true;

        yield return new WaitForSeconds(duration);

        PlayerStats.Instance.MovementSpeed -= currentBoostAmount;
        PlayerController.canShowSpeedLines = false;

        currentBoostAmount = 0;
        boostCoroutine = null;
    }
}