using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ExplodingEnemy : EnemyBase
{
    [SerializeField] GameObject fire_VFX;
    [SerializeField] float timeBeforeExpo = 2;
    public bool isBuffed = false;
    private void Start()
    {
        EnemyBase.no_Enemies += 1;
    }
    public void ExplodeAttack()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(timeBeforeExpo);
            if (!isDead && Vector3.Distance(GetPlayer().position, transform.position) <= defaultAttackRadius)
            {
                ExplosionManager.instance.CreateExplosion(transform, 6, 10, 25);
                SetStateToDead();
                Destroy(gameObject);
            }

            DisableAnyAttackState();
        }
    }
}
