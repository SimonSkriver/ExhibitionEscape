using UnityEngine;

public class MeatUseController : MonoBehaviour
{
    public static MeatUseController Instance { get; private set; }

    [Header("Effect Scripts")]
    [SerializeField] private BoulderStrengthEffect boulderStrengthEffect;

    private void Awake()
    {
        Instance = this;

        if (boulderStrengthEffect == null)
            boulderStrengthEffect = GetComponent<BoulderStrengthEffect>();
    }

    public void UseMeat(MeatData meatData)
    {
        if (meatData == null)
            return;

        PlayerStats.Instance.AddHealth(meatData.healthAmount);

        if (meatData.enablesBoulderBreaking && boulderStrengthEffect != null)
        {
            boulderStrengthEffect.StartBoulderStrength(meatData.boulderBreakingDuration);
        }
    }

    public void SpawnBone(MeatData meatData)
    {
        if (meatData.bonePrefab == null) return;

        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 0.05f;

        Instantiate(meatData.bonePrefab, spawnPosition, Quaternion.identity);
    }
}