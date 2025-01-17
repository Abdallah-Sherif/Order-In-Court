using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using URC.Core;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour
{
    public static int no_Enemies = 0;


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
    [SerializeField] protected GameObject[] projectiles;

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
    [Range(0, 20)]
    public float defaultAttackCoolDown;
    [SerializeField] private UnityEvent defaultAttackEvent;
    [Range(0, 20)]
    [SerializeField] private float specialAttackCoolDown;
    [SerializeField] private UnityEvent specialAttackEvent;
    [SerializeField] bool hasSpecial = false;

    bool canDefaultAttack = true;
    bool canSpecialAttack = true;
    [Header("34an sleem 2aly a3ml header")]
    [SerializeField] UnityEvent onStun;
    [SerializeField] UnityEvent onUnStun;

    [SerializeField] private string corner;

    public bool isDead = false;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        NMP = new NavMeshPath();
        target = transform.position;
    }

    public void Dead()
    {
        animator.SetTrigger("isDeath");
        speed = 0;
        acceleration = 0;
        state = State.Dead;
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

        if (NMP.corners.Length <= 1)
            return;

        foreach (var item in NMP.corners)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawCube(item , new Vector3(.1f, .1f, .1f));
        }
    }

    public void FixedUpdate()
    {

        if (Motor.instance.isEnemyOff) return;

        if (state == State.Dead) { rb.isKinematic = true;  enabled = false; }

        UpdateNavMeshPath();

        void UpdateNavMeshPath()
        {
            if (isStopped)
                return;

            NavMeshHit hitR;
            NavMesh.SamplePosition(transform.position, out hitR , 50000 , -1);
            NavMesh.CalculatePath(hitR.position, target, NavMesh.AllAreas, NMP);

            for (int i = 0; i < NMP.corners.Length - 1; i++)
                Debug.DrawLine(NMP.corners[i], NMP.corners[i + 1], Color.red);
        }

        bool braking = true;

        if (isDead) return;
        Move();

        void Move()
        {
            if (NMP.corners.Length < 2 || state == State.Stun)
                return;

            if (Vector3.Distance(transform.position, target) > distanceToStop)
            {
                LookAt(NMP.corners[1], 15);
                rb.AddForce((NMP.corners[1] - NMP.corners[0]).normalized * speed * Time.deltaTime, ForceMode.VelocityChange);

                braking = true;
            }
            else
            {
                if (braking == true)
                {
                    Debug.Log("brake");
                    rb.AddForce(-rb.velocity.normalized * Time.deltaTime * (speed / 4), ForceMode.VelocityChange);
                    braking = false;
                }
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
            dummyAim.transform.forward = nearestPlayer.transform.position - transform.position;

            Debug.DrawRay(dummyAim.transform.position, dummyAim.transform.forward, Color.red);
        }

        DummyAim();

        //

        if (state == State.Stun)
        {
            Debug.Log("sds");
            animator.gameObject.transform.forward = -rb.velocity;
        }

        if (isAttacking || state == State.Stun || state == State.Dead) 
        { return; }


        float nearp_p = Vector3.Distance(nearestPlayer.position, transform.position);
        if (nearp_p <= detectionRadius)
        {
            
            if (nearp_p <= defaultOuterAttackRadius && nearp_p >= defaultAttackRadius)
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
        if(hasSpecial && (state == State.chasing || state == State.defaultAttack) && canSpecialAttack)
        {
            SpecialAttack();
            state = State.specialAttack;
        }
    }
    public void SetStateToDead()
    {     
        isDead = true;
        no_Enemies -= 1;
        state = State.Dead;
    }
    IEnumerator AbilityCooldown(Ability ability)
    {
        yield return new WaitForSeconds(ability.coolDown);
        ability.canCast = true;
    }
    public void GetStunned()
    {
        state = State.Stun;    
        StartCoroutine(stunDelay());
    }
    IEnumerator stunDelay()
    {
        onStun.Invoke();
        animator.SetBool("isStunned" , true);

        float delay = 0;

        if (CheckGround())
        {
            delay = 3;
        }
        yield return new WaitForSeconds(1f + delay);

        if (!CheckGround())
        {
            StartCoroutine(stunDelay());
            yield break;
        }

        animator.gameObject.transform.localEulerAngles = Vector3.zero;
        animator.SetBool("isStunned", false);
        onUnStun.Invoke();
        state = State.chasing;
    }

    void Animate()
    {
        if (state == State.defaultAttack || state == State.specialAttack)
            return;

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
            state = State.chasing;
            yield return new WaitForSeconds(specialAttackCoolDown);

            canSpecialAttack = true;
        }
    }

    public bool CheckGround()
    {
        float _distanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(transform.position, Vector3.down, _distanceToTheGround + 0.1f);
    }

    void DefaultAttack()
    {
        isAttacking = true;
        StartCoroutine(delay());

        IEnumerator delay()
        {
            LookAt(GetPlayer().position, 100000);

            //StopMovement();
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
        state = State.Null;
    }
    //

    public GameObject SummonProjectile(int projectileID, float Force , bool DestroyLater)
    {
        if (hasProjectile == false)
            return null;

        GameObject g = Instantiate(projectiles[projectileID], dummyAim.transform.position , Quaternion.LookRotation( GetPlayer().transform.position - transform.position));
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

    public void Die()
    {
        Destroy(gameObject);
    }
}

public enum State
{
    patrolling,
    chasing,
    defaultAttack,
    specialAttack,
    Stun,
    Dead,
    Null
}

