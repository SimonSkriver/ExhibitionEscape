using UnityEngine;

[CreateAssetMenu(fileName = "NewAxe", menuName = "Scriptable Objects/Items/Axe")]
public class AxeData : ItemData
{
    [Header("Axe Settings")]
    public float reach = 5f;
}