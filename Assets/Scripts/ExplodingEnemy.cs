using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBehaviourScript
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
