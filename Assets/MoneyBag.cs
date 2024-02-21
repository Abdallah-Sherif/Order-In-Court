using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : MonoBehaviour
{
    private float impact;
    private float radius;
    private int damage;
    public void setData(float impact,float radius,int damage)
    {
        this.impact= impact;
        this.radius= radius;
        this.damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        ExplosionManager.instance.CreateExplosion(this.transform, radius, impact,damage);
        Destroy(gameObject);
    }
}
