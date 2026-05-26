using UnityEngine;

[CreateAssetMenu(menuName = "FootSteps/Terrain FootStep Set")]
public class FootStepSO : ScriptableObject
{
   public TerrainLayer terrainLayer;
    public AudioClip[] footstepClips;
}
