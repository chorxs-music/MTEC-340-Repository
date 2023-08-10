using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class PlayerShoot : MonoBehaviour
{
    private Camera _camera;
    private bool canFire;
    private bool canDie;
    private float fireCooldown = 2.0f;
    private GameBehaviour _gameManager;

    [SerializeField] GameObject _playerBullet;
    [SerializeField] int _bulletNumber = 5;
    [SerializeField] float _bulletSpeed = 8.0f;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _shoot;

    void Start()
    {
        _camera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canFire = true;
        canDie = false;
        _gameManager = FindAnyObjectByType<GameBehaviour>();

    }

    void Update()
    {
        if (GameBehaviour.Instance.State == GameStates.GameState.Play)
        {
            if (Input.GetMouseButtonDown(0) && canFire == true && _bulletNumber > 0)
            {
                Vector3 point = new(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);

                Ray ray = _camera.ScreenPointToRay(point);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject objHit = hit.transform.gameObject;
                    Debug.Log("Hit Bullet!");
                    //EnemyTarget target = objHit.GetComponent<EnemyTarget>();
                    _bulletNumber--;
                    _gameManager.Fire();
                    StartCoroutine(BulletIndicator(hit.point));
                }
            }
        }
    }

    private void OnGUI()
    {
        int size = 50;
        float posX = _camera.pixelWidth / 2 - size / 2;
        float posY = _camera.pixelHeight / 2 - size / 2;

        GUI.contentColor = Color.red;

        GUI.Label(new Rect(posX, posY, size, size), "+");
    }

    IEnumerator BulletIndicator(Vector3 pos)
    {
        if (GameBehaviour.Instance.State == GameStates.GameState.Play)
        {
            Debug.Log("Spawning Bullet");
            GameObject bullet = Instantiate(_playerBullet, transform.TransformPoint(Vector3.forward * 0.5f), transform.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = transform.forward * _bulletSpeed;
            _source.PlayOneShot(_shoot);
            canFire = false;
            yield return new WaitForSeconds(3.0f);
            Destroy(bullet);
            if (_bulletNumber <= 0 && GameBehaviour.Instance.State != GameStates.GameState.Victory)
            {
                _gameManager.Death();
            }
            yield return new WaitForSeconds(fireCooldown);
            canFire = true;
        }
    }

    public void RefillBullets()
    {
        _bulletNumber = 5;
        Debug.Log("Refilled Bullets!");
    }

    public void DeclareDeathAbility()
    {
        Debug.Log("You can die now!");
        canDie = true;
    }
}
