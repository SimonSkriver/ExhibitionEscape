using UnityEngine;

[CreateAssetMenu(fileName = "NewCoconut", menuName = "Scriptable Objects/Items/Food/Coconut")]
public class CoconutData : FoodData
{
    [Header("Coconut Settings")]
    public float saturationAmount = 50f;

    [Header("Boost Settings")]
    public float MovementSpeedBoost = 5f;
}
