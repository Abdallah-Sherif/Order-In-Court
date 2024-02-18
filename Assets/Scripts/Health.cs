using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent onHit;
    public UnityEvent onDeath;

    int health = 100;
    public int maxHealth;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("HIT");
        if(health <= 0)
        {
            onDeath.Invoke();
            return;
        }
        onHit.Invoke();
    }
}
