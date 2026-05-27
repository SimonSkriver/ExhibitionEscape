using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FootSteps/Terrain FootStep Set")]
public class FootStepSO : ScriptableObject
{
   public TerrainLayer terrainLayer;
    public List<AudioClip> footstepClips = new List<AudioClip>();
}
