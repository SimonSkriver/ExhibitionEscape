using UnityEngine;

[CreateAssetMenu(fileName = "NewFruit", menuName = "Scriptable Objects/Items/Fruit")]
public class FruitData : ItemData
{
    [Header("Health")]
    public int healthAmount = 10;

    [Header("Movement Boost")]
    public bool givesMovementBoost;
    public float movementBoostAmount;
    public float movementBoostDuration;

    [Header("Jump Boost")]
    public bool givesJumpBoost;
    public float jumpBoostAmount;
    public float jumpBoostDuration;

    [Header("Spawn Object On Use")]
    public bool spawnsObjectOnUse;
    public GameObject objectToSpawn;
}