using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hammer : MonoBehaviour
{
    Animator anim;
    private float timeSinceLastSlow = 0f;
    private float timewhenslowed = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSlow = Time.time - timewhenslowed;
        anim.SetBool("isAttack", Input.GetKey(KeyCode.Mouse0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemie" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HammerSwing")
        {
            Vector3 hitDir = other.transform.position - transform.position;
            other.GetComponent<Rigidbody>().AddForce(hitDir * 10f, ForceMode.Impulse);
            if(timeSinceLastSlow > 0.5f) StartCoroutine(TimeEfect());
            Debug.Log("SMASH");
            Debug.Log(timeSinceLastSlow);
        }
    }
    
    IEnumerator TimeEfect() 
    {
        anim.speed = 0.01f;
        timewhenslowed= Time.time;
        yield return new WaitForSeconds(0.2f);
        anim.speed = 1f;
    }
}
