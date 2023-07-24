using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    public GameState State;
    public PlayerState Pstate;
    public GameObject bulletPrefab;

    private GameBehaviour gameBehaviour;
    private BulletAudio audioManager;
    private Coroutine _doubleSpeed;
    private float _xLimit = 8.287f;
    private float _yLimit = 4.13f;
    
    [SerializeField] float _shipspeed = 5.0f;
    [SerializeField] KeyCode _leftKey;
    [SerializeField] KeyCode _rightKey;
    [SerializeField] KeyCode _upKey;
    [SerializeField] KeyCode _downKey;
    [SerializeField] KeyCode _spaceBar;
    [SerializeField] AudioClip _bulletFire;
    [SerializeField] AudioClip _speedBoost;
    [SerializeField] float _speedBoostDuration = 7.0f;
    [SerializeField] GameObject _playerShip;

    AudioSource _source;
    private object instantiatedObject;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehaviour.Instance.State == GameState.Play)
        {
            if (Input.GetKey(_leftKey) && transform.position.x >= -_xLimit)
            {
                transform.position -= new Vector3(_shipspeed, 0, 0) * Time.deltaTime;
            }

            if (Input.GetKey(_rightKey) && transform.position.x <= _xLimit)
            {
                transform.position += new Vector3(_shipspeed, 0, 0) * Time.deltaTime;
            }

            if (Input.GetKey(_upKey) && transform.position.y <= _yLimit)
            {
                transform.position += new Vector3(0, _shipspeed, 0) * Time.deltaTime;
            }

            if (Input.GetKey(_downKey) && transform.position.y >= -_yLimit)
            {
                transform.position -= new Vector3(0, _shipspeed, 0) * Time.deltaTime;
            }

            if (Input.GetKeyDown(_spaceBar))
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        if (GameBehaviour.Instance.State == GameState.Play)
        {
            Vector3 BulletSpawnPosition = transform.position + new Vector3(0.444f, 0.5553f, 0);
            GameObject Bullet = Instantiate(bulletPrefab, BulletSpawnPosition, Quaternion.identity);

            Rigidbody2D instantiatedRigidbody = Bullet.GetComponent<Rigidbody2D>();
            instantiatedRigidbody.simulated = true;
            instantiatedRigidbody.useFullKinematicContacts = true;

            audioManager = GameObject.Find("AudioManager").GetComponent<BulletAudio>();
            if (audioManager == null)
            {
                Debug.Log("No sound for bullet clone!");
            }

            BulletBehaviour bulletBehaviour = Bullet.GetComponent<BulletBehaviour>();

            if (bulletBehaviour != null)
            {
                bulletBehaviour.Fire();
                _source.PlayOneShot(_bulletFire);
            }
        }
    }

    public void doubleSpeed()
    {
        _shipspeed = 12.0f;
        _source.PlayOneShot(_speedBoost);
        if (_doubleSpeed != null)
        {
            StopCoroutine(_doubleSpeed);
        }
        _doubleSpeed = StartCoroutine(PowerupDoubleSpeed());
    }

    IEnumerator PowerupDoubleSpeed()
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        Pstate = PlayerState.Default;
        _shipspeed = 5.0f;
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}