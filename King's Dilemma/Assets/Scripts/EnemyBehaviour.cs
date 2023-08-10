using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private bool canSeePlayer;
    private readonly float _sphereRadius = 0.75f;
    private bool _isAlive;
    private int _locationIndex = 0;
    private NavMeshAgent _agent;
    private Vector3 angleDirection;
    private bool canFire;
    private GameBehaviour _gameManager;
    private Rigidbody rb;
    private float startAngle = -45.0f;

    [SerializeField] float _visionRange = 40.0f;
    [SerializeField] GameObject _enemyBullet;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _enemyShoot;

    public Transform Patrol_Route;
    public Transform PlayerPosition;
    public List<Transform> Locations = new List<Transform>();
    public List<Transform> UnvisitedLocations = new List<Transform>();

    void Start()
    {
        canSeePlayer = false;
        _isAlive = true;
        canFire = true;
        InitializePatrolRoute();
        _agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        _agent.speed = 4.0f;
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        _gameManager = FindAnyObjectByType<GameBehaviour>();
        
        NextPatrolLocation();
        ShuffleLocations();
    }

    void Update()
    {
        if (_isAlive)
        {
            if (_agent.remainingDistance < 0.1f && !_agent.pathPending)
            {
                NextPatrolLocation();
            }
        }

        if (GameBehaviour.Instance.State != GameStates.GameState.Play)
        {
            _agent.speed = 0;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            
        }

        else
        {
            _agent.speed = 4;
            _agent.updateRotation = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        float angleIncrement = 10.0f;
        Vector3 forwardAngle = transform.forward;

        for (int i = 0; i <= 7; i++)
        {
            float angle = startAngle + (i * angleIncrement);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forwardAngle;
            Ray ray = new Ray(transform.position + Vector3.up * 0.5f, direction);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, direction * _visionRange, Color.red);

            if (GameBehaviour.Instance.State == GameStates.GameState.Play)
            {
                if (Physics.SphereCast(ray, _sphereRadius, out RaycastHit hit, _visionRange))
                {
                    GameObject hitObj = hit.transform.gameObject;

                    if (hitObj.GetComponent<CharacterController>())
                    {
                        canSeePlayer = true;
                        _agent.destination = PlayerPosition.position;
                        _agent.speed = 7.0f;
                        if (canFire != false)
                        {
                            StartCoroutine(EnemyFire());
                        }
                        Debug.Log("I see player!");
                    }
                }
            }
        }
    }

    void InitializePatrolRoute()
    {
        foreach (Transform child in Patrol_Route)
        {
            Locations.Add(child);
        }
        UnvisitedLocations.AddRange(Locations);
    }

    void ShuffleLocations()
    {
        //Fisher-Yates shuffle
        for (int i = 0; i < Locations.Count - 1; i++)
        {
            int randomIndex = Random.Range(i, Locations.Count);
            Transform temp = Locations[i];
            Locations[i] = Locations[randomIndex];
            Locations[randomIndex] = temp;
        }

    }

    void NextPatrolLocation()
    {
        if (UnvisitedLocations.Count == 0)
        {
            UnvisitedLocations.AddRange(Locations);
        }

        int randomIndex = Random.Range(0, UnvisitedLocations.Count);
        _agent.destination = UnvisitedLocations[randomIndex].position;

        UnvisitedLocations.RemoveAt(randomIndex);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            _gameManager.EnemyDeathSound();
        }

        if (collision.gameObject.CompareTag("Map"))
        {
            Vector3 normalizedWall = (transform.position - collision.transform.position).normalized;
            Vector3 nudgeVector = normalizedWall * 0.5f;
            transform.position += nudgeVector;
        }
    }

    IEnumerator EnemyFire()
    {
        if (GameBehaviour.Instance.State == GameStates.GameState.Play)
        {
            GameObject enemyBullet = Instantiate(_enemyBullet, transform.TransformPoint(Vector3.forward * 2.0f) + Vector3.up * 0.7f, (transform.rotation));
            _source.PlayOneShot(_enemyShoot);
            canFire = false;
            yield return new WaitForSeconds(3.0f);
            canFire = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _gameManager.Death();
        }
    }
}