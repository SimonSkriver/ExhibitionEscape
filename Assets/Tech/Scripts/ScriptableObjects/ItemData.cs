using UnityEngine;

//[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;

    [Header("Equipped Prefab")]
    public GameObject equippedPrefab;

    [Header("Item Type")]
    public ItemType itemType;
}