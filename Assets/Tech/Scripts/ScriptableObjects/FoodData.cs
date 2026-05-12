using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Items/Food")]
public class FoodData : ItemData
{
    [Header("Food Settings")]
    public float feedAmount;
    public float saturationAmount;
    public float healAmount;
}
