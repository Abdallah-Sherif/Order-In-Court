using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    [SerializeField] int HealthGain = 5;
    [SerializeField] AudioClip HealthSound;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.tag != "Player") return;
        Health health = collision.gameObject.GetComponent<Health>();
        if (health.health >= 100) return;
        AudioFxManager.instance.PlaySoundEffect(HealthSound, transform, 1);
        health.health += HealthGain;
        Destroy(gameObject);
    }
}
