using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int amount = 1;
    [Tooltip("How many seconds before spawning the next")]
    [SerializeField] int spawnInterval = 2;

    [SerializeField] bool isSpawningInRuntime = true;

    void Start() => StartCoroutine(SpawnPrefabs());

    IEnumerator SpawnPrefabs() {
        for (int i = 0; i < amount; ++i) {
            Instantiate(prefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnInterval);
        }

        if (isSpawningInRuntime) StartCoroutine(SpawnCooldown());
    }
    
    IEnumerator SpawnCooldown() {
        float cooldown = Random.Range(10, 100);
        Debug.Log("Spawning in: " + cooldown);
        yield return new WaitForSeconds(cooldown);
        amount = Random.Range(0, 3);
        Debug.Log("Spawning " + amount + " seagulls");
        StartCoroutine(SpawnPrefabs());
    }
}
