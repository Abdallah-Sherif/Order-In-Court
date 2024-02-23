using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float spawnRate = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPacks());

    }

    IEnumerator SpawnPacks()
    {
        yield return new WaitForSeconds(spawnRate);
        int index = Random.Range(0, spawnPoints.Length);
        HealthPackManager.instance.CreateHealthPack(spawnPoints[index].position);
        StartCoroutine(SpawnPacks());
    }
}
