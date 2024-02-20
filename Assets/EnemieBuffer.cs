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
        Mathf.Lerp(col_trans.localScale.x, col_trans.localScale.x * 2, Time.deltaTime);
        Mathf.Lerp(col_trans.localScale.y, col_trans.localScale.y * 2, Time.deltaTime);
        Mathf.Lerp(col_trans.localScale.z, col_trans.localScale.z * 2, Time.deltaTime);
        health.health += (int)(health.health * 0.5f);
        yield return new WaitForSeconds(buffTime);
        Mathf.Lerp(col_trans.localScale.x, col_scale.x, Time.deltaTime);
        Mathf.Lerp(col_trans.localScale.y, col_scale.y, Time.deltaTime);
        Mathf.Lerp(col_trans.localScale.z, col_scale.z, Time.deltaTime);
        health.health -= (int)(health.health * 0.5f);
    }
}
