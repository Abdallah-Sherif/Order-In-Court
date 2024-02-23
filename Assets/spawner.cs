using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float spawnrate;
    [SerializeField] int spawnLimit = 20;
    [SerializeField] float spawnRadius;
    [SerializeField] Transform otherspawn;
    bool breakOut = true;
    bool canSpawn = true;
    [SerializeField] bool canScale = false;
    [SerializeField] float scaleTime = 25f;
    [SerializeField] int IncreaseFactor = 5;
    [SerializeField] int maxNum = 50;
    void Start()
    {
        EnemyBase.no_Enemies = 0;
        if(canScale) StartCoroutine(IncreaseLimit());
    }
    private void Update()
    {
        if(EnemyBase.no_Enemies < spawnLimit && breakOut == true) 
        {
            StartCoroutine(delay());
            breakOut = false;
        }
    }
    IEnumerator IncreaseLimit()
    {
        yield return new WaitForSeconds(scaleTime);
        if (spawnLimit >= maxNum) yield break;
        spawnLimit += IncreaseFactor;
        StartCoroutine(IncreaseLimit());
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(spawnrate);
        breakOut = true;
        Vector3[] bob = new Vector3[2] { otherspawn.position , transform.position };
        Instantiate(enemies[Random.Range(0 , enemies.Length)] , RandomLocationNav(bob[Random.Range(0 , 2)], spawnRadius) , Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, spawnRadius);
        Gizmos.DrawSphere(otherspawn.position, spawnRadius);
    }

    public static Vector3 RandomLocationNav(Vector3 center, float maxDistance)
    {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        return hit.position;
    }
}
