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

        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Reset();
        }
    }

    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Reset();
        predator.movement.speedMult = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        predator.movement.speedMult = 1f;
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
