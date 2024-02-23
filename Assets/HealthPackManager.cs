using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackManager : MonoBehaviour
{
    public static HealthPackManager instance;
    [SerializeField] GameObject healthPackPrefab;
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void CreateHealthPack(Vector3 point)
    {
        int chance = Random.Range(0, 2);
        if (chance == 1)
        {
            GameObject g = Instantiate(healthPackPrefab, point, Quaternion.identity);
        } 
    }
}
