using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingEnemy : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shooPos;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletDelay = 5f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float detectionRangearget = Mathf.Infinity;
    Transform player;
    bool check = true;
    bool canShoot = true;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        aimHandler();
        checkShoot();
        shooting();
    }
    void aimHandler()
    {
        transform.LookAt(player.position);
    }
    void shooting()
    {
        if (check)
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
    }
    void checkShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRangearget, obstacleLayer))
            check = false;
        else
            check = true;
    }
    IEnumerator shootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(bulletDelay);
        canShoot = true;
    }
}