using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Scriptable Objects/Items/Food")]
public class FoodData : ItemData
{
    [Header("(Hover below) Heal amount 0-100")]
    [Tooltip (
    "Banana = 30\n" +
    "Coconut = 20\n" +
    "Meat = 50\n" +
    "Berry = 10" )]
    [Range(0, 100)]
    public int healAmount;

    [Space]
    [Header("(Hover below) Boost amount")]
    [Tooltip (
    "Banana = Jump Boost (5) \n" +
    "Coconut = Movement Speed Boost (10) \n" +
    "Meat = Strength (10) \n" +
    "Berry = 0")]
    public float boostAmount;
}
