using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieMelee : EnemyBase
{
    [Header("Attack Properties")]
    [SerializeField] float enemieAttackRate = 1f;
    public int meleeDamage = 25;
    public void AttackPlayer()
    {
        GetPlayer().GetComponent<Health>().TakeDamage(meleeDamage);
        DisableAnyAttackState();
    }
    public void SpeicalAttack()
    {

    }
}
