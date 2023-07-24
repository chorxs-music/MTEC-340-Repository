using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;

    public GameState State;
    public PlayerState Pstate;
    public int RemainingLives;
    public int Score;

    [SerializeField] TextMeshProUGUI _playerScore;
    [SerializeField] TextMeshProUGUI _playerLives;
    [SerializeField] TextMeshProUGUI _playerUI;
    [SerializeField] GameObject _playerShipPrefab;
    [SerializeField] AudioClip _playerHit;
    [SerializeField] AudioClip _playerDeath;
    [SerializeField] AudioClip _spawnWave;
    [SerializeField] AudioClip _plusLife;
    [SerializeField] AudioSource _source;
    [SerializeField] GameObject _scout;
    [SerializeField] GameObject _destroyer;
    [SerializeField] GameObject menuButton;

    static float _xSpawnLimit = 8.2f;
    static float _ySpawnLowerLimit = 1.46f;
    static float _ySpawnUpperLimit = 4.41f;

    private float EnemySpawnDuration = 5.0f;

    int shipStart = 2;
    int scoutSpawn = 2;
    int destroyerSpawn = 1;

    void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        State = GameState.Play;
        RemainingLives = 2;
        Score = 0;
        _playerLives.enabled = true;
        _playerScore.enabled = true;
        _playerUI.text = "Paused";
        _playerUI.enabled = false;
        _playerScore.text = "Score: " + Score.ToString();
        scoutSpawn = 2;
        destroyerSpawn = 1;
        menuButton.SetActive(false);
        UpdateLivesText();
        StartCoroutine(SpawnEnemies());

        for (int i = 0; i < shipStart; i++)
        {
            float randomX = Random.Range(-_xSpawnLimit, _xSpawnLimit);
            float randomY = Random.Range(_ySpawnLowerLimit, _ySpawnUpperLimit);
            Vector3 scoutShipStart = new Vector3(randomX, randomY);
            Vector3 destroyerShipStart = new Vector3(randomX, randomY);
            GameObject _scoutStart = Instantiate(_scout, scoutShipStart, Quaternion.identity);
            GameObject _destroyerStart = Instantiate(_destroyer, destroyerShipStart, Quaternion.identity);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && State != GameState.GameOver)
        {
            State = State == GameState.Play ? GameState.Pause : GameState.Play;
            _playerUI.text = "Paused";
            _playerUI.enabled = !_playerUI.enabled;
        }

        if (State == GameState.GameOver && Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void ScoutDestroyed()
    {
        Score += 5;
        _playerScore.text = "Score: " + Score.ToString();
    }

    public void DestroyerDestroyed()
    {
        Score += 10;
        _playerScore.text = "Score: " + Score.ToString();
    }

    public void onContact()
    {
        RemainingLives--;

        if (RemainingLives <= 0)
        {
            BulletAudio audioManager = FindObjectOfType<BulletAudio>();
            ShipBehaviour shipBehaviour = FindObjectOfType<ShipBehaviour>();
            audioManager.playerDeath();
            UpdateLivesText();
            shipBehaviour.Death();
            GameOver();
        }

        else 
        {
            UpdateLivesText();
            _source.PlayOneShot(_playerHit);
        }
    }

    private void UpdateLivesText()
    {
        _playerLives.text = "Lives: " + RemainingLives.ToString();
    }

    public void GameOver()
    {
        State = GameState.GameOver;
        _playerUI.enabled = true;
        _playerUI.text = "Game Over! Press R to Restart";
        Debug.Log("Game Over");
        menuButton.SetActive(true);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void ResetGame()
    {
        RemoveProjectiles();
        RemoveEnemyShips();
        RemoveSpeedBoost();
        RemoveLife();
        Vector3 PlayerSpawn = new Vector3(0, -4.13f);
        GameObject _playership = Instantiate(_playerShipPrefab, PlayerSpawn, Quaternion.identity);
        Rigidbody2D instantiatedRigidBody = _playership.GetComponent<Rigidbody2D>();
        instantiatedRigidBody.simulated = true;
        instantiatedRigidBody.useFullKinematicContacts = true;
        _playerUI.text = "Paused";
        _playerUI.enabled = false;
        Debug.Log("Game Restarted!");
        Start();
    }

    public void addLife()
    {
        RemainingLives++;
        _source = GetComponent<AudioSource>();
        _source.PlayOneShot(_plusLife);
        UpdateLivesText();
    }

    IEnumerator SpawnEnemies()
    {
        while (State == GameState.Play)
        {
            for (int i = 0; i < scoutSpawn; i++)
            {
                float randomX = Random.Range(-_xSpawnLimit, _xSpawnLimit);
                float randomY = Random.Range(_ySpawnLowerLimit, _ySpawnUpperLimit);
                Vector3 scoutSpawnPoint = new Vector3(randomX, randomY);
                GameObject ScoutSpawn = Instantiate(_scout, scoutSpawnPoint, Quaternion.identity);
            }

            for (int i = 0; i < destroyerSpawn; i++)
            {
                float randomX = Random.Range(-_xSpawnLimit, _xSpawnLimit);
                float randomY = Random.Range(_ySpawnLowerLimit, _ySpawnUpperLimit);
                Vector3 destroyerSpawnPoint = new Vector3(randomX, randomY);
                GameObject ScoutSpawn = Instantiate(_destroyer, destroyerSpawnPoint, Quaternion.identity);
            }

            scoutSpawn++;
            destroyerSpawn++;

            _source.PlayOneShot(_spawnWave);

            yield return new WaitForSeconds(EnemySpawnDuration);
        }
    }

    public void RemoveProjectiles()
    {
        GameObject[] Projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject Projectile in Projectiles)
        {
            Destroy(Projectile);
        }
    }

    public void RemoveEnemyShips()
    {
        GameObject[] EnemyShips = GameObject.FindGameObjectsWithTag("EnemyShip");

        foreach (GameObject EnemyShip in EnemyShips)
        {
            Destroy(EnemyShip);
        }
    }

    public void RemoveSpeedBoost()
    {
        GameObject[] DoubleSpeeds = GameObject.FindGameObjectsWithTag("DoubleSpeed");

        foreach (GameObject DoubleSpeed in DoubleSpeeds)
        {
            Destroy(DoubleSpeed);
        }
    }

    public void RemoveLife()
    {
        GameObject[] AddLifes = GameObject.FindGameObjectsWithTag("AddLife");

        foreach (GameObject AddLife in AddLifes)
        {
            Destroy(AddLife);
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR

        EditorApplication.isPlaying = false;

#else
        Application.Quit();

#endif
    }
}
