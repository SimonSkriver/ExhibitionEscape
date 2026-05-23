using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMeat", menuName = "Scriptable Objects/Items/Meat")]
public class MeatData : FoodData
{
    [System.Serializable]
    public class MeatStage
    {
        [Header("Inventory Image")]
        public Sprite inventoryIcon;

        [Header("Meat Prefab")]
        public GameObject heldPrefab;
    }

    [Header("Meat Stages")]
    [Tooltip("Order: Full to just before bone")]
    public List<MeatStage> meatStages = new List<MeatStage>();
}