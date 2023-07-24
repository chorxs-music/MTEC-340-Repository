using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{

    public static GameBehaviour Instance;

    public GameState State;
    public PlayerState Pstate;

    private ShipBehaviour shipBehaviour;
    private GameBehaviour gameBehaviour;
    private float speedMinSpawnDelay = 20.0f;
    private float speedMaxSpawnDelay = 35.0f;
    private float addLifeMinSpawnDelay = 30.0f;
    private float addLifeMaxSpawnDelay = 45.0f;
    [SerializeField] GameObject _speedIcon;
    [SerializeField] GameObject _lifeIcon;

    static float minSpawnX = -8.9f;
    static float maxSpawnX = 8.9f;
    static float minSpawnY = -4.6f;
    static float maxSpawnY = -1.6f;

    private float randomX;
    private float randomY;

    void Start()
    {
        Pstate = PlayerState.Default;
        StartCoroutine(SpeedSpawn());
        StartCoroutine(LifeSpawn());
    }

    IEnumerator SpeedSpawn()
    {
        while (State == GameState.Play)
        {
            randomX = Random.Range(minSpawnX, maxSpawnX);
            randomY = Random.Range(minSpawnY, maxSpawnY);
            Vector3 SpawnPosition = new Vector3(randomX, randomY);
            yield return new WaitForSeconds(Random.Range(speedMinSpawnDelay, speedMaxSpawnDelay));
            Pstate = PlayerState.DoubleSpeed;
            GameObject DoubleSpeed = Instantiate(_speedIcon, SpawnPosition, Quaternion.identity);
            Rigidbody2D instantiatedRigidBody = DoubleSpeed.GetComponent<Rigidbody2D>();
            instantiatedRigidBody.simulated = true;
            Debug.Log("Spawned Double Speed!");
        }
    }

    IEnumerator LifeSpawn()
    {
        while (State == GameState.Play)
        {
            randomX = Random.Range(minSpawnX, maxSpawnX);
            randomY = Random.Range(minSpawnY, maxSpawnY);
            Vector3 SpawnPosition = new Vector3(randomX, randomY);
            yield return new WaitForSeconds(Random.Range(addLifeMinSpawnDelay, addLifeMaxSpawnDelay));
            GameObject AddLife = Instantiate(_lifeIcon, SpawnPosition, Quaternion.identity);
            Rigidbody2D instantiatedRigidBody = AddLife.GetComponent<Rigidbody2D>();
            instantiatedRigidBody.simulated = true;
            Debug.Log("Spawned Life!");
        }
    }
}
