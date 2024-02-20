using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : EnemyBase
{
    [SerializeField] GameObject fire_VFX;
    [SerializeField] float timeBeforeExpo = 2;
    public void ExplodeAttack()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(timeBeforeExpo);

            if (Vector3.Distance(GetPlayer().position, transform.position) <= defaultAttackRadius)
            {
                Instantiate(fire_VFX, transform.position, Quaternion.identity, null);
                Destroy(gameObject);
            }

            DisableAnyAttackState();
        }
    }
}
