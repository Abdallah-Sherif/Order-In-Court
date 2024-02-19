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
    [SerializeField] LayerMask enemieLayerMask;
    Ability ability0, ability1, ability2;
    bool abilityInProgress = false;
    [SerializeField] Transform _playerModel;
    [Header("Ground Pound Properties")]
    [SerializeField] float impactPower = 10f;
    // Update is called once per frame
    private void IntializeAbilites()
    {
        ability0 = new Ability();
        ability1 = new Ability();
        ability2 = new Ability();
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips) 
        {
            if(clip.name == "swing smooth")
            {
                ability0.abilityDuration = clip.length;
                break;
            }
        }
        ability0.coolDown = ability0.abilityDuration - 0.4f;
        ability0.abilityLogicStart = delegate
        {
            anim.SetBool("isAttack", true);
        };
        ability0.abilityLogicStop = delegate
        {
            anim.SetBool("isAttack", false);
        };

        ability1.abilityDuration = 1f;
        ability1.coolDown = 1f;
        ability1.abilityLogicStart = delegate
        {
            ThrowEnemies();
        };
        ability1.abilityLogicStop = delegate
        {
            
        };

    }
    void ThrowEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(_playerModel.position + _playerModel.forward * 5, 6,enemieLayerMask);
        foreach(Collider collider in hits)
        {
            Debug.Log(collider.gameObject.name);
            collider.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * impactPower, ForceMode.Impulse);
        }    
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_playerModel.position + _playerModel.forward * 5, 6);
    }
    private void Start()
    {
        IntializeAbilites();
    }
    void Update()
    {
        timeSinceLastSlow = Time.time - timewhenslowed;

        if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Q) && !abilityInProgress && ability1.canCast)
        {
            AbilityStart(ability1);
        }else if (Input.GetKey(KeyCode.Mouse0) && !abilityInProgress && ability0.canCast)
        {
            AbilityStart(ability0);
        }

        anim.SetFloat("moveSpeed", Mathf.Lerp(anim.GetFloat("moveSpeed") , Motor.instance.GetComponent<Rigidbody>().velocity.magnitude / 6f , 5 * Time.deltaTime) );
        anim.SetBool("isJump", !Motor.instance.Grounded);
    }
    void AbilityStart(Ability ability)
    {
        StartCoroutine(AbilityDuration(ability));
    }
    IEnumerator AbilityDuration(Ability ability)
    {
        //ability logic
        ability.abilityLogicStart.Invoke();
        if (ability.interuptsAbilites) abilityInProgress = true;
        ability.canCast = false;
        yield return new WaitForSeconds(ability.abilityDuration);
        ability.abilityLogicStop.Invoke();
        abilityInProgress = false;
        StartCoroutine(AbilityCooldown(ability));
    }
    IEnumerator AbilityCooldown(Ability ability)
    {
        yield return new WaitForSeconds(ability.coolDown);
        ability.canCast = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if (abilityInProgress&&other.transform.tag == "Enemie" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "swing smooth")
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

