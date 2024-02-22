using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public UnityEvent onHit;
    public UnityEvent onDeath;

    public int health = 100;
    public int maxHealth;

    public bool isBuffed = false;

    [SerializeField] AudioClip playerDeathClip;
    [SerializeField] Slider playerHealthSlider;

    private void Update()
    {
        if(playerDeathClip != null)playerHealthSlider.value = health;
    }
    public void TakeDamage(int damage,string weaponUsed = "")
    {
        if (health <= 0) return;
        health -= damage;
        if(health <= 0)
        {
            onDeath.Invoke();
            if(playerDeathClip == null) Destroy(gameObject,3);
            return;
        }
        if(weaponUsed == "Hammer") onHit.Invoke();
    }
    public void onPlayerDeath()
    {
        AudioFxManager.instance.PlayPlayerFX(playerDeathClip, 3f, true);
    }
}
