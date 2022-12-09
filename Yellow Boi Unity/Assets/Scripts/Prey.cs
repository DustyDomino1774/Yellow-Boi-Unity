using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Prey : MonoBehaviour
{
    public Movement movement { get; private set; }
    public AnimatedSprite death;
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public float radius = 20;
    private AIPath aiPath;
    public int nodeDistance;

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
        ai.canMove = false;
        Vector3 nextPosition;
        Quaternion nextRotation;
        if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            ai.destination = PickRandomPoint();
            ai.SearchPath();
            ai.MovementUpdate(Time.deltaTime, out nextPosition, out nextRotation);
            Debug.Log("Next postion: " + nextPosition);
            // using what the next direction should depending on the pathing when he reaches the node
            // if (Mathf.Abs(nextPosition.y))

            //if statement if not at next position in list of pathfinding change direction 
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            this.movement.SetDirection(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            this.movement.SetDirection(Vector2.left);
        }

        //rotate sprite based off angle to face direction for responsive feel
        //math really did become useful
        float rotAngle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
        this.transform.rotation = Quaternion.AngleAxis(rotAngle * Mathf.Rad2Deg, Vector3.forward); // rads so convert to degree
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
