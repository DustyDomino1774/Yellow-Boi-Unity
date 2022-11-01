using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassagePort : MonoBehaviour
{
    public Transform connection;

    private void OnTriggerEnter2D(Collider2D swap)
    {
        Vector3 position = swap.transform.position;
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;
        swap.transform.position = position;
    }
}
