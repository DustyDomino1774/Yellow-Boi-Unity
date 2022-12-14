using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorFrightened : PredatorBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    public bool eaten { get; private set; }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    private void Eaten()
    {
        eaten = true;
        predator.SetPosition(predator.home.inside.position);
        predator.home.Enable(duration);

        this.body.enabled = false;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    private void Flash()
    {
        if (!eaten)
        {
            this.blue.enabled = false;
            this.white.enabled = true;
            this.white.GetComponent<AnimatedSprite>().Reset();
        }
    }

    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Reset();
        this.predator.movement.speedMult = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        this.predator.movement.speedMult = 1f;
        eaten = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Prey"))
        {
            if (enabled)
            {
                Eaten();
            }
        }
    }
}
