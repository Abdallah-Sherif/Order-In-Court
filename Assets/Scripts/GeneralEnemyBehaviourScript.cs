using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviourScript : MonoBehaviour
{
    Transform player;
    NavMeshAgent enemy;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = gameObject.GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        enemy.SetDestination(player.transform.position);
    }
}
