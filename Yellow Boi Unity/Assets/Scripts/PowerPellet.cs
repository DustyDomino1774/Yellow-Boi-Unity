using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 6f;
    protected override void Eaten()
    {
        FindObjectOfType<GameManager>().EatPowerPellet(this);
    }

}
