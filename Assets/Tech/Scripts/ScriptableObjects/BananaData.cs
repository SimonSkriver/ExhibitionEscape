using UnityEngine;

[CreateAssetMenu(fileName = "NewBanana", menuName = "Scriptable Objects/Items/Food/Banana")]
public class BananaData : FoodData
{
    [Header("Boost Settings")]
    [Range(0, 100)] //To be adjusted
    public float JumpBoost = 5f;
}
