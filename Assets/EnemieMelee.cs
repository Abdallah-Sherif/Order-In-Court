using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieMelee : EnemyBase
{
    [Header("Attack Properties")]
    [SerializeField] float enemieAttackRate = 1f;
    public int meleeDamage = 25;
    public bool isBuffed = false;
    private void Start()
    {
        EnemyBase.no_Enemies += 1;
    }
    public void AttackPlayer()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1f);
            float dist = Vector3.Distance(transform.position, GetPlayer().transform.position);
            if(dist <= defaultAttackRadius)
            {
                GetPlayer().GetComponent<Health>().TakeDamage(meleeDamage);
            }
            DisableAnyAttackState();
        }
    }
    public void SpeicalAttack()
    {

    }
}
