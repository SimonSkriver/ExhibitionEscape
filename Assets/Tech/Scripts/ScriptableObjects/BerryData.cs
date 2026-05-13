using UnityEngine;

[CreateAssetMenu(fileName = "NewBerry", menuName = "Scriptable Objects/Items/Food/Berry")]
public class BerryData : FoodData
{
    [Header("Berry Settings")]
    public float saturationAmount = 20f;
}
