using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Unity.VisualScripting.Member;

public class GameBehaviour : MonoBehaviour
{
    private int _playerLife = 1;
    private int _bulletNumber = 5;
    private EnemySpawn _spawner;
    private PlayerBehaviour _playerBehaviour;
    private PlayerSpawn _playerSpawn;
    private PlayerShoot _playerShoot;

    [SerializeField] TextMeshProUGUI _playerLives;
    [SerializeField] TextMeshProUGUI _playerBullets;
    [SerializeField] TextMeshProUGUI _gameMenu;
    [SerializeField] KeyCode _pauseGame;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _death;
    [SerializeField] AudioClip _enemyDeath;

    public GameStates.GameState State;
    public static GameBehaviour Instance;

    void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        State = GameStates.GameState.Play;
        _playerShoot = FindAnyObjectByType<PlayerShoot>();
        _playerLives.enabled = true;
        _playerBullets.enabled = true;
        _gameMenu.enabled = false;
        _playerLife = 1;
        _bulletNumber = 5;
        _playerLives.text = "Lives: " + _playerLife;
        _playerBullets.text = "Bullets: " + _bulletNumber;
    }

    void Update()
    {
        if (Input.GetKeyDown(_pauseGame) && State != GameStates.GameState.Gameover)
        {
            State = State == GameStates.GameState.Play ? GameStates.GameState.Pause : GameStates.GameState.Play;
            _gameMenu.text = "Paused";
            _gameMenu.enabled = !_gameMenu.enabled;
            Debug.Log("Paused");
        }

        if (State == GameStates.GameState.Gameover && Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void Fire()
    {
        _bulletNumber--;
        _playerBullets.text = "Bullets: " + _bulletNumber;
    }

    public void Death()
    {
        _playerLife = 0;
        _playerLives.text = "Life: " + _playerLife;
        State = GameStates.GameState.Gameover;
        DeathSound();
        _gameMenu.text = "Game Over!";
        _gameMenu.enabled = true;
    }

    void DeathSound()
    {
        _source.PlayOneShot(_death);
    }

    public void Victory()
    {
        State = GameStates.GameState.Victory;
        _gameMenu.text = "You've slain the king!";
        _gameMenu.enabled = true;
    }

    public void RefillBullets()
    {
        _bulletNumber = 5;
        Debug.Log("Refilled Bullets!");
        _playerBullets.text = "Bullets: " + _bulletNumber;
        _playerShoot.RefillBullets();
    }

    void ResetGame()
    {
        _playerBehaviour = FindAnyObjectByType<PlayerBehaviour>();
        _playerBehaviour.ResetPlayer();

        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Enemy2 in Enemies)
        {
            Destroy(Enemy2);
        }

        _spawner = FindAnyObjectByType<EnemySpawn>();
        _spawner.SpawnEnemies();
        Start();
    }

    public void EnemyDeathSound()
    {
        _source.PlayOneShot(_enemyDeath);
    }
}
