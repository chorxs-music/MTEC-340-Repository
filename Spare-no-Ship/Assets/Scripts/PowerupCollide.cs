using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupCollide : MonoBehaviour
{
    public static GameBehaviour Instance;

    public GameState State;
    public PlayerState Pstate;

    private ShipBehaviour shipBehaviour;
    private GameBehaviour gameBehaviour;

    private void Start()
    {
        shipBehaviour = FindAnyObjectByType<ShipBehaviour>();
        gameBehaviour = FindAnyObjectByType<GameBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("AddLife"))
            {
                Debug.Log("Entered Life");
                gameBehaviour.addLife();
                Destroy(this.gameObject);
            }

            if (gameObject.CompareTag("DoubleSpeed"))
            {
                Debug.Log("Entered Speed");
                Pstate = PlayerState.DoubleSpeed;
                shipBehaviour.doubleSpeed();
                Destroy(this.gameObject);
            }
        }
    }
}
