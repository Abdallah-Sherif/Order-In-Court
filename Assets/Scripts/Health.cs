using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent onHit;
    public UnityEvent onDeath;

    public int health = 100;
    public int maxHealth;

    public void TakeDamage(int damage,string weaponUsed = "")
    {
        health -= damage;
        if(health <= 0)
        {
            onDeath.Invoke();
            Destroy(gameObject,3);
            return;
        }
        if(weaponUsed == "Hammer") onHit.Invoke();
    }
}
