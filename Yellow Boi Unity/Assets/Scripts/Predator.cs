using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    public Movement movement { get; private set; }
    public PredatorHome home { get; private set; }
    public PredatorFrightened frightened { get; private set; }
    public PredatorBehavior initialBehavior;
    public int playerNumber;
    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<PredatorHome>();
        frightened = GetComponent<PredatorFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        switch (playerNumber)
        {
            case 1:
                if ( Input.GetKeyDown(KeyCode.W))
                {
                    this.movement.SetDirection(Vector2.up);
                }
                else if ( Input.GetKeyDown(KeyCode.S))
                {
                    this.movement.SetDirection(Vector2.down);
                }
                else if ( Input.GetKeyDown(KeyCode.D))
                {
                    this.movement.SetDirection(Vector2.right);
                }
                else if ( Input.GetKeyDown(KeyCode.A))
                {
                    this.movement.SetDirection(Vector2.left);
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.UpArrow) )
                {
                    this.movement.SetDirection(Vector2.up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) )
                {
                    this.movement.SetDirection(Vector2.down);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) )
                {
                    this.movement.SetDirection(Vector2.right);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) )
                {
                    this.movement.SetDirection(Vector2.left);
                }
                break;
            default:
                break;
        }

    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();

        if (home != initialBehavior)
        {
            home.Disable();
        }

        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Prey"))
        {
            if (frightened.enabled)
                FindObjectOfType<GameManager>().PredatorCaught(this);
            else
                FindObjectOfType<GameManager>().PreyCaught();
        }
    }
}
