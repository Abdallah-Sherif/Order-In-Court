using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testMeshAgent : MonoBehaviour
{
    Transform player;
    NavMeshAgent navMeshAgent;
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //navMeshAgent.SetDestination(player.transform.position);
    }
}
