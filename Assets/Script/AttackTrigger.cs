using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigge : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Hurt();
        }
    }
}
