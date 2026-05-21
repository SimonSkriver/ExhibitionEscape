using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int amount = 1;
    [Tooltip("How many seconds before spawning the next")]
    [SerializeField] int spawnInterval = 2;

    void Start() => StartCoroutine(SpawnPrefabs());

    IEnumerator SpawnPrefabs() {
        for (int i = 0; i < amount; ++i) {
            Instantiate(prefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
