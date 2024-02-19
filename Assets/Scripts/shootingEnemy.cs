using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingEnemy : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shooPos;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletDelay = 5f;
    Transform player;
    bool canShoot = true;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        aimHandler();
        shooting();
    }
    void aimHandler()
    {
        transform.LookAt(player.position);
    }
    void shooting()
    {
        if (!canShoot) return;
        else
        {
            GameObject bulletOnj = Instantiate(bullet, shooPos.position, transform.rotation);
            bulletOnj.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            Destroy(bulletOnj, 5f);
            StartCoroutine(shootDelay());   
        }
    }

    IEnumerator shootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(bulletDelay);
        canShoot = true;
    }
}