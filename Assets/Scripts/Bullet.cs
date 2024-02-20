using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    string tag;
    [SerializeField] UnityEvent onHit;
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
        ChangeLayer(this.gameObject, 0);
    }
    void ChangeLayer(GameObject targetObject, int newLayer)
    {
        targetObject.layer = newLayer;
        foreach (Transform child in targetObject.transform)
        {
            ChangeLayer(child.gameObject, newLayer);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isDying) return;
        if (activateExpo)
        {
            onHit.Invoke();
            Destroy(gameObject, 1.5f);
        }else
        {
            Destroy(gameObject);
        }
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().isTrigger= true;
        isDying= true;
        if (collision.transform.tag == tag ) return;
        Health health = collision.gameObject.GetComponent<Health>();
        if (health == null) return;
        health.TakeDamage(100);

    }
}
