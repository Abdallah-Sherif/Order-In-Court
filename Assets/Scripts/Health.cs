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
    [SerializeField] AudioSource lowHealthAudioSource;

    [SerializeField] bool isObject = false;
    private void Update()
    {
        if (playerDeathClip != null)
        {
            playerHealthSlider.value = health;
            if(health <= 25 && !lowHealthAudioSource.isPlaying)
            {
                lowHealthAudioSource.Play();
            }else if(health > 25 && lowHealthAudioSource.isPlaying)
            {
                lowHealthAudioSource.Stop();
            }
        }
    }
    public void TakeDamage(int damage,string weaponUsed = "")
    {
        if (health <= 0) return;
        health -= damage;
        if(health <= 0)
        {
            if (isObject) Destroy(gameObject);
            onDeath.Invoke();
            if (playerDeathClip == null)
            {

                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(3);
                    Destroy(gameObject);
                    HealthPackManager.instance.CreateHealthPack(transform.position);
                }
            }
            return;
        }
        if(weaponUsed == "Hammer") onHit.Invoke();
    }
    public void onPlayerDeath()
    {
        AudioFxManager.instance.PlayPlayerFX(playerDeathClip, 1f, true);
    }
}
