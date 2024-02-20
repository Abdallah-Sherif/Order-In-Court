using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieBuffer : EnemyBase
{
    [SerializeField] LayerMask enemieLayerMask;
    [Header("Buff Properties")]
    [SerializeField] float buffTime = 5f;
    [SerializeField] float bulletSpeed = 1000;
    public void ThrowMoney()
    {
        SummonProjectile(0, bulletSpeed, true);
        DisableAnyAttackState();
    }

    public void BuffEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 6, enemieLayerMask);
        foreach (Collider collider in hits)
        {
            Debug.Log(collider.gameObject.name);
            StartCoroutine(addBuffEffect(collider));
        }
        DisableAnyAttackState();
    }
    IEnumerator addBuffEffect(Collider collider)
    {
        Health health= collider.GetComponent<Health>();
        Transform col_trans = collider.transform;
        Vector3 col_scale = col_trans.localScale;
        col_trans.localScale *=2;
        health.health += (int)(health.health * 0.5f);
        yield return new WaitForSeconds(buffTime);
        
        health.health -= (int)(health.health * 0.5f);
    }
}
