using UnityEngine;

[CreateAssetMenu(fileName = "NewDrink", menuName = "Scriptable Objects/Items/Drink")]
public class DrinkData : ItemData
{
    [Header("Drink Settings")]
    public float saturationAmount; 
}
