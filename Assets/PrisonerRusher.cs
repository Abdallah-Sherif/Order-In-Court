using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerRusher : EnemyBase
{
    private bool isRunning = false;

    [SerializeField] ParticleSystem smoke;
    void Update()
    {
        animator.SetBool("isWalk", isRunning);
        if (!isRunning)
            return;
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position + transform.forward * 1, transform.forward, 4))
        //{
        //StopAllCoroutines();
        //StopRunning();
        //}
    }

    public void MinotaurDefaultAttack()
    {
        GetPlayer().GetComponent<Health>().TakeDamage(10);
        DisableAnyAttackState();
    }


    public void MinotaurSpecialAttack()
    {
        //animator.SetBool("startCharging", true);
        StartCoroutine(StartRunningTime());

        IEnumerator StartRunningTime()
        {
            yield return new WaitForSeconds(2.5f);
            StartRunning();
        }
    }

    void StartRunning()
    {
        speed = speed * 2;
        acceleration = acceleration * 2;
        SetDestination(GetPlayer().transform.position);
        animator.speed = 2;
        //animator.SetTrigger("startRunning");

        isRunning = true;

        smoke.Play();
        StartCoroutine(RunTime());

        IEnumerator RunTime()
        {
            yield return new WaitForSeconds(2.5f);
            StopRunning();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform == GetPlayer() && isRunning)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(50);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(
                Vector3.up * 10 + (collision.transform.position - this.transform.position), ForceMode.Impulse);
        }
    }
    void StopRunning()
    {
        //animator.SetTrigger("hasColided");
        animator.speed = 1;
        smoke.Stop();
        isRunning = false;
        speed = speed / 2;
        acceleration = acceleration / 2;
        StartCoroutine(Stunned());


        IEnumerator Stunned()
        {
            StopMovement();
            yield return new WaitForSeconds(2);
            DisableAnyAttackState();
        }
    }
}
