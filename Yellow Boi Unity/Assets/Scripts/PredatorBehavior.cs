using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Predator))]
public class PredatorBehavior : MonoBehaviour
{
    public Predator predator { get; private set; }
    public float duration;

    private void Awake()
    {
        predator = GetComponent<Predator>();
    }

    public void Enable()
    {
        Enable(duration);
    }

    public virtual void Enable(float duration)
    {
        enabled = true;

        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }

    public virtual void Disable()
    {
        enabled = false;

        CancelInvoke();
    }
}
