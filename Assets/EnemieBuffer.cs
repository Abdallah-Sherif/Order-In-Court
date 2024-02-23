using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieBuffer : EnemyBase
{
    [SerializeField] LayerMask enemieLayerMask;
    [Header("Buff Properties")]
    [SerializeField] float buffTime = 5f;
    [SerializeField] float bulletSpeed = 1000;

    List<Collider> buffedEnemies = new List<Collider>();
    private void Start()
    {
        EnemyBase.no_Enemies += 1;
    }
    public void ThrowMoney()
    {
        if(!CheckGround() || state == State.Dead || state == State.Stun)
        {
            DisableAnyAttackState();
            return;
        }
        SummonProjectile(0, bulletSpeed, true);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.8f);
            DisableAnyAttackState();
        }
    }
    public void BuffEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 6, enemieLayerMask);
        foreach (Collider collider in hits)
        {
            if (collider == this.GetComponent<CapsuleCollider>() || collider.GetComponent<Health>().isBuffed) continue;

            collider.GetComponent<Health>().isBuffed = true;
            buffedEnemies.Add(collider);
            Debug.Log(collider.gameObject.name);
            addBuffEffect(collider);
        }
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1.5f);
            DisableAnyAttackState();
        }
    }
    void addBuffEffect(Collider collider)
    {
        Health health= collider.GetComponent<Health>();
        Transform col_trans = collider.transform;
        Vector3 col_scale = col_trans.localScale;
        col_trans.localScale *=1.2f;
        health.health += (int)(health.health * 0.5f);       
    }
}
