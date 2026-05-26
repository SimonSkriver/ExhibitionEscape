using UnityEngine;

//[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;

    [Header("Equipped Prefab")]
    public GameObject equippedPrefab;
    public Vector3 positionOffSet;
    public Vector3 rotationOffset;

    [Header("Item Type")]
    public ItemType itemType;
}