using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossTrigger : MonoBehaviour
{

    BoxCollider triggerBox;
    [SerializeField] UnityEvent onTrigger;
    [SerializeField] AudioClip CEOEntranceClip;
    // Start is called before the first frame update
    void Start()
    {
        triggerBox= GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player") return;
        AudioFxManager.instance.PlayPlayerFX(CEOEntranceClip, 1f, true);
        onTrigger.Invoke();
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
