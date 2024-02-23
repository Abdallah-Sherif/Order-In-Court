using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using URC.Core;
public class Uzi : MonoBehaviour
{
    [Header("Bullet Properties")]
    [SerializeField] Transform _shootPos;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletSpread;
    [SerializeField] float _bulletFireRate;
    [SerializeField] KeyCode _shootKey;
    [SerializeField] GameObject _bulletPrefab;
    [Header("Ability Properties")]
    [SerializeField] float ability1Duration;
    [SerializeField] float ability1Cooldown;
    public bool abilityInProgress = false;
    private Ability ability0, ability1,ability2;
    [SerializeField]
    List<AudioClip> abilitySounds;
    [SerializeField] Animator anim;
    [SerializeField] AudioClip shootAudioClip;

    public static Uzi instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void IntializeAbilites()
    {
        ability0 = new Ability();
        ability1 = new Ability();
        ability2 = new Ability();
        ability0.abilityDuration = 0;
        ability0.coolDown = _bulletFireRate;
        ability0.interuptsAbilites = false;
        ability0.abilityLogicStart = delegate
        {
            anim.SetBool("isShoot", true);
            ShootBullets();
        };
        ability0.abilityLogicStop = delegate
        {
            
        };
        ability1.interuptsAbilites = false;
        ability1.abilityDuration = ability1Duration;
        ability1.coolDown = ability1Cooldown;
        ability1.abilityLogicStart = delegate 
        {
            AudioFxManager.instance.PlayPlayerFX(abilitySounds[UnityEngine.Random.Range(0, abilitySounds.Count)], 1f, true);
            ability0.coolDown /= 2;
            anim.speed= 2;
        };
        ability1.abilityLogicStop = delegate 
        {
            ability0.coolDown *= 2;
            anim.speed =1;
        };

    }
    // Start is called before the first frame update
    void Start()
    {
        IntializeAbilites();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isShoot", Input.GetKey(_shootKey));
        if (Input.GetKey(_shootKey) && !abilityInProgress && ability0.canCast)
        {
            AbilityStart(ability0);
        }
        if (Input.GetKey(_shootKey)&&Input.GetKey(KeyCode.Q) && !abilityInProgress && ability1.canCast)
        {
            AbilityStart(ability1);
        }
    } 
    void ShootBullets()
    {
        AudioFxManager.instance.PlaySoundEffect(shootAudioClip, this.transform, 1);
        float x_offset = UnityEngine.Random.Range(-_bulletSpread, _bulletSpread);
        float y_offset = UnityEngine.Random.Range(-_bulletSpread, _bulletSpread);
        Vector3 dir = transform.forward + new Vector3(x_offset, y_offset, 0);
        dir.Normalize();
        GameObject bullet = Instantiate(_bulletPrefab, _shootPos.position, Quaternion.LookRotation(dir));
        bullet.GetComponent<Rigidbody>().velocity = dir * _bulletSpeed;
        bullet.tag = this.transform.tag;
        if (ability0.coolDown < _bulletFireRate) bullet.GetComponent<Bullet>().activateExpo = true;
    }
    void AbilityStart(Ability ability)
    {
        StartCoroutine(AbilityDuration(ability));
    }
    IEnumerator AbilityDuration(Ability ability)
    {
        //ability logic
        ability.abilityLogicStart.Invoke();
        if(ability.interuptsAbilites) abilityInProgress = true;
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

}
public class Ability
{
    public Action abilityLogicStart;
    public Action abilityLogicStop;
    public float coolDown;
    public float abilityDuration;

    public string abilityName = "NoName (please assign a name)";

    [HideInInspector] public UnityEvent[] events;

    [HideInInspector] public bool canCast = true;
    [HideInInspector] public bool skipNextCoolDown = false;
    [HideInInspector] public bool isDisabled = false;

    public bool interuptsAbilites = true;

    public void DisableAbility(bool b)
    {
        isDisabled = b;
    }
}
