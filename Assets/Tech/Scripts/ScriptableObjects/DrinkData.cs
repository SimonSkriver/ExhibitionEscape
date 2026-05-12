using UnityEngine;

[CreateAssetMenu(fileName = "NewDrink", menuName = "Items/Drink")]
public class DrinkData : ItemData
{
    [Header("Drink Settings")]
    public float saturationAmount; 
    public float healAmount;
}
