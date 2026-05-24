using UnityEngine;

public class FruitUseController : MonoBehaviour
{
    public static FruitUseController Instance { get; private set; }

    [Header("Effect Scripts")]
    [SerializeField] private MovementBoostEffect movementBoostEffect;
    [SerializeField] private JumpBoostEffect jumpBoostEffect;
    [SerializeField] private SpawnOnUseEffect spawnOnUseEffect;

    private void Awake()
    {
        Instance = this;

        if (movementBoostEffect == null)
            movementBoostEffect = GetComponent<MovementBoostEffect>();

        if (jumpBoostEffect == null)
            jumpBoostEffect = GetComponent<JumpBoostEffect>();

        if (spawnOnUseEffect == null)
            spawnOnUseEffect = GetComponent<SpawnOnUseEffect>();
    }

    public void UseFruit(FruitData fruitData, Transform playerTransform)
    {
        if (fruitData == null)
            return;

        PlayerStats.Instance.AddHealth(fruitData.healthAmount);

        if (fruitData.givesMovementBoost && movementBoostEffect != null)
        {
            movementBoostEffect.StartBoost(fruitData.movementBoostAmount,fruitData.movementBoostDuration);
        }

        if (fruitData.givesJumpBoost && jumpBoostEffect != null)
        {
            jumpBoostEffect.StartBoost(fruitData.jumpBoostAmount,fruitData.jumpBoostDuration);
        }

        if (fruitData.spawnsObjectOnUse && spawnOnUseEffect != null)
        {
            spawnOnUseEffect.SpawnBelowPlayer(fruitData.objectToSpawn, playerTransform);
        }
    }
}