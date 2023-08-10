using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerBehaviour : MonoBehaviour
{

    [AddComponentMenu("Control Script/FPS Input")]

    private CharacterController _controller;
    private int _playerLife = 1;
    private float _speed = 4.0f;
    private float _mouseSensitivity = 250.0f;
    private GameBehaviour _gameManager;
    private bool _isPowerUpActive = false;
    private float _vertRot = 0;
    private PlayerShoot _playerShoot;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    public Transform PlayerSpawnPoint;

    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _minVertAngle = -45.0f;
    [SerializeField] float _maxVertAngle = 45.0f;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _transform;

    void Start()
    {
        _playerLife = 1;
        _controller = GetComponent<CharacterController>();
        _gameManager = FindAnyObjectByType<GameBehaviour>();
        _transform = GameObject.Find("Player(Clone)").transform;
        _playerShoot = GetComponentInChildren<PlayerShoot>();
    }

    void Update()
    {

        if (GameBehaviour.Instance.State == GameStates.GameState.Play)
        {
            float deltaX = Input.GetAxis("Horizontal") * _speed;
            float deltaZ = Input.GetAxis("Vertical") * _speed;

            Debug.Log("Speed = " + _speed);

            Vector3 movement = new(deltaX, 0, deltaZ);

            movement = Vector3.ClampMagnitude(movement, _speed);
            movement.y = _gravity;
            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement);

            _controller.Move(movement);

            _vertRot -= Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
            _vertRot = Mathf.Clamp(_vertRot, _minVertAngle, _maxVertAngle);

            float xRotation = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            float horizontalRot = transform.localEulerAngles.y + xRotation;

            transform.localEulerAngles = new Vector3(_vertRot, horizontalRot, 0);

            if (Input.GetKey(KeyCode.LeftShift) && _isPowerUpActive == true)
            {
                Speed = 16.0f;
            }

            else if (_isPowerUpActive == true)
            {
                Speed = 12.0f;
            }

            else if (Input.GetKey(KeyCode.LeftShift) && _isPowerUpActive == false)
            {
                Speed = 8.0f;
            }

            else
            {
                Speed = 4.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameBehaviour.Instance.State == GameStates.GameState.Play)
        {
            if (collision.gameObject.CompareTag("EnemyBullet"))
            {
                _playerLife--;
                _gameManager.Death();
                Debug.Log("Player has been killed!");
            }
        }
    }

    public void RefillBullets()
    {
        if (_playerShoot != null)
        {
            _playerShoot.RefillBullets();
        }
        else
        {
            Debug.LogError("PlayerShoot component not found!");
        }
    }

    public void DoubleSpeed()
    {
        _isPowerUpActive = true;
        StartCoroutine(SpeedBoost());
    }

    IEnumerator SpeedBoost()
    {
        _isPowerUpActive = true;
        yield return new WaitForSeconds(5);
        _isPowerUpActive = false;
    }

    public void ResetPlayer()
    {
        _controller.enabled = false;
        Debug.Log("Resetting Player...");
        Vector3 playerSpawnPosition = new Vector3(-44.6f, 0.2f, -45.8f);
        _transform.position = playerSpawnPosition;
        _controller.enabled = true;
    }
}
