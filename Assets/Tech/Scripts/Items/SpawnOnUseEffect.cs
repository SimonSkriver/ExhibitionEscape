using UnityEngine;

public class SpawnOnUseEffect : MonoBehaviour
{
    public void SpawnBelowPlayer(GameObject prefab, Transform playerTransform)
    {
        if (prefab == null || playerTransform == null)
            return;

        Vector3 spawnPosition = playerTransform.position;
        spawnPosition.y += 0.05f;

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}