using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    public Transform PlayerSpawnPoint;


    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject PlayerPrefab = GameObject.FindGameObjectWithTag("Player");
        if (PlayerPrefab != null)
        {
            Destroy(PlayerPrefab);
        }
        GameObject Player = Instantiate(_playerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
    }
}
