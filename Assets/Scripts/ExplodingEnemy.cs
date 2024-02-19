using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBehaviourScript
{
    [SerializeField] GameObject fire_VFX;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameObject explode = Instantiate(fire_VFX, transform.position, transform.rotation);
        }
    }
}
