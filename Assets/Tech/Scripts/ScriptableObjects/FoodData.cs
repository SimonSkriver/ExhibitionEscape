using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Scriptable Objects/Items/Food")]
public class FoodData : ItemData
{
    [Header("Heal Settings")]
    [Range(0, 100)]
    public float healAmount;
}
