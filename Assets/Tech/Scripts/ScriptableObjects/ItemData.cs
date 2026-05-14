using UnityEngine;

//[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basic info")]
    [Tooltip("Name to be displayed in inventory")]
    public string itemName = "New Item";

    [Space]
    public ItemType itemType = ItemType.Other;

    [Space]
    [Tooltip("Picture to be displayed in inventory")]
    public Sprite icon;

    [Space]
    [Tooltip("Item to be spawned in hand when equipping")]
    public GameObject itemPrefab;
}