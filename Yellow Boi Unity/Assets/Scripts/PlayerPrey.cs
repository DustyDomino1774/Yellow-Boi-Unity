using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrey : MonoBehaviour
{
    public Movement movement { get; private set; }
    public AnimatedSprite death;
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.collider = GetComponent<Collider2D>();
        this.movement = GetComponent<Movement>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8) ){
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6) )
        {
            this.movement.SetDirection(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) )
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
