using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossTrigger : MonoBehaviour
{

    BoxCollider triggerBox;
    [SerializeField] UnityEvent onTrigger;
    // Start is called before the first frame update
    void Start()
    {
        triggerBox= GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player") return;
        onTrigger.Invoke();
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
