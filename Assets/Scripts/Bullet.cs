using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    string tag;
    [SerializeField] UnityEvent onHit;
    [SerializeField] int bulletLayer;
    bool isDying = false;
    public bool activateExpo = false;
    void Start()
    {
        Destroy(gameObject,10);
        StartCoroutine(ChangeLayer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ChangeLayer()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeLayer(this.gameObject);
    }
    void ChangeLayer(GameObject targetObject)
    {
        targetObject.layer = bulletLayer;
        foreach (Transform child in targetObject.transform)
        {
            ChangeLayer(child.gameObject);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (isDying) return;
        if (activateExpo)
        {
            onHit.Invoke();
            ExplosionManager.instance.CreateExplosion(transform, 1, 2, 25,true);
            Destroy(gameObject, 1.5f);
        }else
        {
            Destroy(gameObject);
        }
        GetComponent<Rigidbody>().isKinematic = true;
        isDying= true;
        if (collision.transform.tag == tag ) return;
        Health health = collision.gameObject.GetComponent<Health>();
        if (health == null) return;
        health.TakeDamage(10);

    }
}
