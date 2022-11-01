using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrey : MonoBehaviour
{
    public Movement movement { get; private set; }

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)){
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
}
