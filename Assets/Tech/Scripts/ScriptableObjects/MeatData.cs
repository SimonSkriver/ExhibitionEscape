using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMeat", menuName = "Scriptable Objects/Items/Meat")]
public class MeatData : ItemData
{
    [System.Serializable]
    public class MeatStage
    {
        [Header("Inventory Image")]
        public Sprite inventoryIcon;

        [Header("Held Prefab")]
        public GameObject heldPrefab;
    }

    [Header("Health")]
    public int healthAmount = 10;

    [Header("Boulder Breaking")]
    public bool enablesBoulderBreaking = true;
    public float boulderBreakingDuration = 20f;

    [Header("Meat Stages")]
    [Tooltip("Order: Full meat to almost eaten")]
    public List<MeatStage> meatStages = new List<MeatStage>();

    public GameObject bonePrefab;

    public Sprite GetInventoryIcon(int stage)
    {
        if (meatStages == null || meatStages.Count == 0)
            return icon;

        stage = Mathf.Clamp(stage, 0, meatStages.Count - 1);

        if (meatStages[stage].inventoryIcon == null)
            return icon;

        return meatStages[stage].inventoryIcon;
    }

    public GameObject GetHeldPrefab(int stage)
    {
        if (meatStages == null || meatStages.Count == 0)
            return equippedPrefab;

        stage = Mathf.Clamp(stage, 0, meatStages.Count - 1);

        if (meatStages[stage].heldPrefab == null)
            return equippedPrefab;

        return meatStages[stage].heldPrefab;
    }

    public bool IsLastStage(int stage)
    {
        if (meatStages == null || meatStages.Count == 0)
            return true;

        return stage >= meatStages.Count - 1;
    }
}