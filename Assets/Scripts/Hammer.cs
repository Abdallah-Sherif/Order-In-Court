using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using URC.Core;

public class Hammer : MonoBehaviour
{
    [SerializeField] Animator anim;
    private float timeSinceLastSlow = 0f;
    private float timewhenslowed = 0;

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSlow = Time.time - timewhenslowed;
        anim.SetBool("isAttack", Input.GetKey(KeyCode.Mouse0));

        anim.SetFloat("moveSpeed", Mathf.Lerp(anim.GetFloat("moveSpeed") , Motor.instance.GetComponent<Rigidbody>().velocity.magnitude / 6f , 5 * Time.deltaTime) );
        anim.SetBool("isJump", !Motor.instance.Grounded);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if (other.transform.tag == "Enemie" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "swing smooth")
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
