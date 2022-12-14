using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;

public class Prey : MonoBehaviour
{
    PlayerControls controls;
    public Movement movement { get; private set; }
    public AnimatedSprite death;
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public float radius = 20;
    private AIPath aiPath;
    public int nodeDistance;
    public Transform[] predators;

    //temporary variable to debugginig player and AI
    public bool activeAI;

    private IAstarAI ai;



    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.collider = GetComponent<Collider2D>();
        this.movement = GetComponent<Movement>();
        this.aiPath = GetComponent<AIPath>();
        this.ai = GetComponent<IAstarAI>();

    }
    
    private Vector3 BFSRandomPoint()
    {
        // Find closest walkable node
        var startNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        var nodes = PathUtilities.BFS(startNode, nodeDistance);
        var singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0];
        var multipleRandomPoints = PathUtilities.GetPointsOnNodes(nodes, 100);

        singleRandomPoint.y = 0;
        singleRandomPoint += ai.position;

        return singleRandomPoint;
    }

    Vector3 PickRandomPoint()
    {
        var point = Random.insideUnitSphere * radius;

        point.y = 0;
        point += ai.position;
        return point;
    }

    // Update is called once per frame
    private void Update()
    {
        //ai.canMove = false;
        //Vector3 nextPosition;
        //Quaternion nextRotation;
        /*if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            ai.destination = PickRandomPoint();
            //ai.SearchPath();
            ai.MovementUpdate(Time.deltaTime, out nextPosition, out nextRotation);
            Debug.Log("Next postion: " + nextPosition);

            // this.movement.SetDirection(nextPosition); //only goes down once and nothing else.
            // using what the next direction should depending on the pathing when he reaches the node
            // if (Mathf.Abs(nextPosition.y))

            //Simply follow node path and interpolate

            //if statement if not at next position in list of pathfinding change direction 
        }*/

        if (!activeAI)
        {
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                this.movement.SetDirection(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                this.movement.SetDirection(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                this.movement.SetDirection(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                this.movement.SetDirection(Vector2.left);
            }

            if (Gamepad.all[0].dpad.up.isPressed)
            {
                this.movement.SetDirection(Vector2.up);
            }
            else if (Gamepad.all[0].dpad.down.isPressed)
            {
                this.movement.SetDirection(Vector2.down);
            }
            else if (Gamepad.all[0].dpad.right.isPressed)
            {
                this.movement.SetDirection(Vector2.right);
            }
            else if (Gamepad.all[0].dpad.left.isPressed)
            {
                this.movement.SetDirection(Vector2.left);
            }
        }

        //rotate sprite based off angle to face direction for responsive feel
        //math really did become useful
        float rotAngle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
        this.transform.rotation = Quaternion.AngleAxis(rotAngle * Mathf.Rad2Deg, Vector3.forward); // rads so convert to degree
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activeAI)
        {
            Node node = other.GetComponent<Node>();

            // Do nothing while the ghost is frightened
            if (node != null && enabled)
            {
                Vector2 direction = Vector2.zero;
                float maxDistance = float.MaxValue;

                // Find the available direction that moves farthest from predators/ghost
                foreach (Vector2 availableDirection in node.availableDirections)
                {
                    // If the distance in this direction is greater than the current
                    // min distance then this direction becomes the new closest
                    Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);

                    float[] distances = new float[predators.Length];

                    for (int i = 0; i < predators.Length; i++)
                    {
                        distances[i] = (this.predators[i].position - newPosition).sqrMagnitude;

                        if (distances[i] > maxDistance)
                        {
                            direction = availableDirection;
                            maxDistance = distances[i];
                        }
                    }
                }

                this.movement.SetDirection(direction);
            }
        }
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        death.enabled = false;
        death.spriteRenderer.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        death.enabled = true;
        death.spriteRenderer.enabled = true;
        death.Reset();
    }
}
