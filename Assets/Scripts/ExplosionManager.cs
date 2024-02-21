using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager instance;
    [Header("Explosion Prefab")]
    [SerializeField] GameObject ExplosionSFX;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null) instance = this;
    }
    void Start()
    {
        
    }
    public void CreateExplosion(Transform point,float radius,float impact,int damage)
    {
        GameObject newExplosion = Instantiate(ExplosionSFX,point.position,Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(point.position, radius);
        foreach (Collider collider in hits)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            Health health = collider.GetComponent<Health>();
            if(rb == null) continue;

            if(health != null)
            {
                health.TakeDamage(damage);
            }
            Vector3 dir = collider.transform.position - point.position;
            rb.AddForce(dir * impact, ForceMode.Impulse);
        }
    }
}
