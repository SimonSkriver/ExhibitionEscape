using UnityEngine;

[CreateAssetMenu(fileName = "NewMeat", menuName = "Scriptable Objects/Items/Food/Meat")]
public class MeatData : FoodData
{
    [Header("Meat Settings")]
    public float saturationAmount = 10f;

    [Header("Boost Settings")]
    public float strengthBoost = 5f;
    public int useAmount = 5;
}
