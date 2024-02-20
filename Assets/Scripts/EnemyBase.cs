using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using URC.Core;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour
{

    private NavMeshPath NMP;
    private bool isStopped = false;
    private bool isAttacking = false;
    private Vector3 target;

    private Rigidbody rb;

    public State state = State.patrolling;
    public Animator animator;

    [Header("Projectiles")]
    [SerializeField] private bool hasProjectile;
    public GameObject dummyAim;
    [SerializeField] private GameObject[] projectile;

    [Header("Navigation")] [SerializeField]
    private float detectionRadius;

    [SerializeField] private float defaultOuterAttackRadius;
    [SerializeField] public float defaultAttackRadius;
    [SerializeField] private float specialAttackRadius;
    [SerializeField] private LayerMask groundLayerMask;
    [Header("RigidBody")] 
    public float speed = 1000;
    public float acceleration = 10;
    [SerializeField] private float distanceToStop = 4;

    [Header("EnemyProperties")]
    [Range(2, 20)]
    [SerializeField] private float defaultAttackCoolDown;
    [SerializeField] private UnityEvent defaultAttackEvent;
    [Range(2, 20)]
    [SerializeField] private float specialAttackCoolDown;
    [SerializeField] private UnityEvent specialAttackEvent;

    bool canDefaultAttack = true;
    bool canSpecialAttack = true;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        NMP = new NavMeshPath();
        target = transform.position;
    }

    private void OnDrawGizmos()
    {
        //draw gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, defaultAttackRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, defaultOuterAttackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, specialAttackRadius);
    }

    public void Update()
    {
        UpdateNavMeshPath();

        void UpdateNavMeshPath()
        {
            if (isStopped)
                return;

            NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, NMP);

            for (int i = 0; i < NMP.corners.Length - 1; i++)
                Debug.DrawLine(NMP.corners[i], NMP.corners[i + 1], Color.red);
        }

        Move();

        void Move()
        {
            if (NMP.corners.Length == 0)
                return;

            if (Vector3.Distance(transform.position, target) > distanceToStop)
            {
                LookAt(NMP.corners[1], 5);
                rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime, ForceMode.Force);
            }

            if (rb.velocity.magnitude > acceleration)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, acceleration);
            }
        }

        Animate();

        //

        Transform nearestPlayer = GetPlayer();

        void DummyAim()
        {
            dummyAim.transform.LookAt(nearestPlayer);

            Debug.DrawRay(dummyAim.transform.position, dummyAim.transform.forward, Color.red);
        }

        DummyAim();

        //

        if (isAttacking)
        { return; }
            

        float nearp_p = Vector3.Distance(nearestPlayer.position, transform.position);

        if (nearp_p <= detectionRadius)
        {
            if (specialAttackRadius != 0 && canSpecialAttack && nearp_p <= specialAttackRadius && nearp_p > defaultOuterAttackRadius && nearp_p > defaultAttackRadius)
            {
                state = State.specialAttack;
            }
            else if (nearp_p <= defaultOuterAttackRadius && nearp_p >= defaultAttackRadius)
            {
                state = State.chasing;
            }
            else if (canDefaultAttack && nearp_p <= defaultAttackRadius)
            {
                state = State.defaultAttack;
            }
            else if (!(nearp_p <= defaultAttackRadius))
            {
                state = State.chasing;
            }
        }
        else
        {
            state = State.patrolling;
        }
        

        switch (state)
        {
            case State.patrolling:
                Patrol();
                break;
            case State.chasing:
                Chasing();
                break;
            case State.defaultAttack:
                DefaultAttack();
                break;
            case State.specialAttack:
                SpecialAttack();
                break;
        }

    }

    void Animate()
    {
        if (state == State.defaultAttack || state == State.specialAttack)
            return;

        if (animator == null) return;

        animator.SetBool("isWalk", (rb.velocity.magnitude > 0.3f));
        animator.SetFloat("walkSpeed", Mathf.Clamp(rb.velocity.magnitude, 0, 1));
    }
    
    private float nextRun = -1;
    private bool isCoroutineStarted = false;
    void Patrol()
    {
        if (rb.velocity.magnitude > 0.5f)
            return;

        if (nextRun > 0)
        {
            nextRun -= Time.deltaTime;
            return;
        }
        else
        {
            if (!isCoroutineStarted)
                StartCoroutine(_delay());
        }

        IEnumerator _delay()
        {
            isCoroutineStarted = true;

            NMP.ClearCorners();
            SetDestination(RandomLocationNav(transform.position, 10));
            yield return new WaitForSeconds(Random.Range(2, 4));
            nextRun = Random.Range(2, 6);

            isCoroutineStarted = false;
        }
    }

    void Chasing()
    {
        SetDestination(GetPlayer().position);
    }

    void SpecialAttack()
    {
        isAttacking = true;
        StartCoroutine(delay());

        IEnumerator delay()
        {
            LookAt(GetPlayer().position, 100000);

            StopMovement();
            specialAttackEvent.Invoke();

            canSpecialAttack = false;

            yield return new WaitForSeconds(specialAttackCoolDown);

            canSpecialAttack = true;
        }
    }

    void DefaultAttack()
    {

        Debug.Log("attackk starteed");

        isAttacking = true;
        StartCoroutine(delay());

        IEnumerator delay()
        {
            LookAt(GetPlayer().position, 100000);

            StopMovement();
            defaultAttackEvent.Invoke();

            canDefaultAttack = false;

            yield return new WaitForSeconds(defaultAttackCoolDown);

            canDefaultAttack = true;
        }
    }

    //this will wait for the child class to finish its attack (spell or melee) then call this  -->
    public void DisableAnyAttackState()
    {
        isAttacking = false;
        //animator.SetBool("isAttack" , false);
        state = State.Null;

        Debug.Log("attackk ended");
    }
    //

    public GameObject SummonProjectile(int projectileID, float Force , bool DestroyLater)
    {
        if (hasProjectile == false)
            return null;

        GameObject g = Instantiate(projectile[projectileID], dummyAim.transform.position , dummyAim.transform.rotation);
        g.GetComponent<Rigidbody>().AddForce(g.transform.forward * Force * Time.deltaTime , ForceMode.Impulse);

        if (DestroyLater)
            Destroy(g , 5);

        return g;
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

    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
        isStopped = true;
        NMP = new NavMeshPath();
    }

    public Transform GetPlayer()
    {
        return Motor.instance.transform;
    }

    public void SetDestination(Vector3 position)
    {
        isStopped = false;

        NavMeshHit hit = new NavMeshHit();
        NavMesh.SamplePosition(position, out hit, 100, NavMesh.AllAreas);
        if (hit.hit)
        {
            target = hit.position;
        }
    }

    public void LookAt(Vector3 pos, float smoothValue)
    {
        Vector3 lookRotation = pos - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Quaternion.LookRotation(lookRotation).eulerAngles.y, 0) , smoothValue * Time.deltaTime);
    }
}

public enum State
{
    patrolling,
    chasing,
    defaultAttack,
    specialAttack,
    Null
}
