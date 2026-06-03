using UnityEngine;
using System.Collections;

public class MeatUseController : MonoBehaviour
{
    public static MeatUseController Instance { get; private set; }

    [Header("Effect Scripts")]
    [SerializeField] private BoulderStrengthEffect boulderStrengthEffect;
    [SerializeField] private bool canEatMeat = true;
    [SerializeField] private float meatEatDelay = 20f;

    private void Awake()
    {
        Instance = this;
        canEatMeat = true;

        if (boulderStrengthEffect == null)
            boulderStrengthEffect = GetComponent<BoulderStrengthEffect>();
    }

    public bool UseMeat(MeatData meatData)
    {
        if (meatData == null)
        {
            Debug.Log("MeatData was null");
            return false;
        }

        if (canEatMeat)
        {
            if (meatData.enablesBoulderBreaking && boulderStrengthEffect != null)
            {
                boulderStrengthEffect.StartBoulderStrength(meatData.boulderBreakingDuration);
                PlayerStats.Instance.AddHealth(meatData.healthAmount);
                StartCoroutine(DisableEnableMeatEating());
                Debug.Log("Ate meat and gave strength effect");
                return true;
            }
            Debug.Log("boulderstrengtheffect is null");
            return false;
        }
        Debug.Log("canEatMeat = false");
        return false;
    }

    private IEnumerator DisableEnableMeatEating()
    {
        canEatMeat = false;
        yield return new WaitForSeconds(meatEatDelay);
        canEatMeat = true;
    }

    public void SpawnBone(MeatData meatData)
    {
        if (meatData.bonePrefab == null) return;

        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 0.05f;

        Instantiate(meatData.bonePrefab, spawnPosition, Quaternion.identity);
    }
}