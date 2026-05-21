using UnityEngine;
using System.Collections;

public class BoarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boarPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 60f;

    public IEnumerator SpawnBoar()
    {
        yield return new WaitForSeconds(respawnDelay);

        Instantiate(boarPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}