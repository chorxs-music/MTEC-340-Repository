using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] EnemySpawnPoints;
    public GameObject EnemyPrefab;
    public int TotalEnemySpawn = 6;

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (EnemySpawnPoints.Length == 0)
        {
            Debug.Log("No spawn points loaded!");
        }

        for (int i = 0; i < TotalEnemySpawn; i++)
        {
            int randomIndex = Random.Range(0, EnemySpawnPoints.Length);
            GameObject SpawnPoint = EnemySpawnPoints[randomIndex];

            Instantiate(EnemyPrefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        }
    }
}
