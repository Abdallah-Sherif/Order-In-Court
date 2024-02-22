using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using URC.Core;

public class Hammer : MonoBehaviour
{
    [SerializeField] Animator anim;
    private float timeSinceLastSlow = 0f;
    private float timewhenslowed = 0;
    [SerializeField] LayerMask enemieLayerMask;
    Ability ability0, ability1, ability2;
    public bool abilityInProgress = false;
    [SerializeField] Transform _playerModel;
    [SerializeField] UnityEvent onHammerHit;
    [SerializeField] float enemieKnockBackImpact = 10f;
    [SerializeField] GameObject impactParticleEffect;
    [Header("Ground Pound Properties")]
    [SerializeField] float impactPower = 10f;
    [SerializeField] List<AudioClip> hammerPoundAudioClips;
    [SerializeField] List<AudioClip> hammerSwooshAudioClips;
    [SerializeField] List<AudioClip> hammerImpactAudioClips;
    [SerializeField] AudioClip hammerClickSFX;
    [SerializeField] AudioClip EnemieDeathSFX;
    public static Hammer instance;
    AudioSource audioSource;
    List<Collider> enemiesAttacked;
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }
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
            AudioFxManager.instance.PlaySoundEffect(hammerSwooshAudioClips[Random.Range(0, hammerSwooshAudioClips.Count)], transform, 2f);
        };
        ability0.abilityLogicStop = delegate
        {
            anim.SetBool("isAttack", false);
        };

        ability1.abilityDuration = 0.4f;
        ability1.coolDown = 2f;
        ability1.abilityLogicStart = delegate
        {
            anim.SetTrigger("hammerPound");
            AudioFxManager.instance.PlayPlayerFX(hammerPoundAudioClips[Random.Range(0, hammerPoundAudioClips.Count)], 3f,true);
            AudioFxManager.instance.PlaySoundEffect(hammerClickSFX, transform, 2f);

        };
        ability1.abilityLogicStop = delegate
        {
            ThrowEnemies();
        };

    }
    void ThrowEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(_playerModel.position + _playerModel.forward * 5, 6,enemieLayerMask);
        foreach(Collider collider in hits)
        {
            collider.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * impactPower, ForceMode.Impulse);
            collider.GetComponent<Health>().TakeDamage(50,"Hammer");
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
        enemiesAttacked = new List<Collider>();
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
        if (!enemiesAttacked.Contains(other) && abilityInProgress&&other.transform.tag == "Enemie" && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "swing smooth")
        {
            GameObject impactEffect = Instantiate(impactParticleEffect, other.transform.position, Quaternion.identity);
            enemiesAttacked.Add(other);
            Vector3 hitDir = other.transform.position - transform.position;
            onHammerHit.Invoke();
            AudioFxManager.instance.PlaySoundEffect(hammerImpactAudioClips[Random.Range(0, hammerImpactAudioClips.Count)], transform, 2f);
            StartCoroutine(EnemieTimeEffect(other,hitDir));
            if (timeSinceLastSlow > 0.5f) StartCoroutine(TimeEfect());
        }
    }
    IEnumerator TimeEfect() 
    {
        anim.speed = 0.01f;
        timewhenslowed= Time.time;
        yield return new WaitForSeconds(0.2f);
        anim.speed = 1f;
    }
    IEnumerator EnemieTimeEffect(Collider collider,Vector3 hitDir)
    {
        Rigidbody rb_temp = collider.GetComponent<Rigidbody>();
        rb_temp.isKinematic = true;
        yield return new WaitForSeconds(0.2f);
        if(rb_temp != null)
        rb_temp.isKinematic = false;
        if (collider == null) yield return null; 
        collider.GetComponent<Health>().TakeDamage(20,"Hammer");
        if(collider.GetComponent<Health>().health <= 0) 
        {
            AudioFxManager.instance.PlaySoundEffect(EnemieDeathSFX, collider.transform, 2f);
        }
        rb_temp.AddForce(hitDir * enemieKnockBackImpact, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        enemiesAttacked.Remove(collider);
    }
}

