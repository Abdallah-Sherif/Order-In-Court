using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] Transform[] shootPoints;
    [SerializeField] float shootFireRate = 1f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float bulletSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(shootBalls());
    }

    IEnumerator shootBalls()
    {
        GameObject ball = Instantiate(projectilePrefab, shootPoints[Random.Range(0,shootPoints.Length)]);
        ball.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        yield return new WaitForSeconds(shootFireRate);
        StartCoroutine(shootBalls());
    }
}
