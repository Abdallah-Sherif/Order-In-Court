using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CEO : EnemyBase
{
    [Header("MoneyBags")]
    [SerializeField] GameObject moneyBagPrefab;
    [SerializeField] float moneySpawnRate;
    [SerializeField] float moneyBagExplosionRadius;
    [SerializeField] float moneyBagExplosionImpact;
    [SerializeField] int moneyBagExplosionDamage;
    [Header("Stock Arrows")]
    [SerializeField] GameObject stockArrowPrefab;
    [SerializeField] float arrowSpawnRate;
    [SerializeField] Transform stockShootPoint;
    [SerializeField] float arrowSpeed;
    [SerializeField] int numberOfArrows = 20;
    [SerializeField] float arrowDistance = 10;
    private int arrowsShot = 0;
    [Header("Melee Attack")]
    [SerializeField] int meleeDamage;
    [SerializeField] float meleeDistance = 3;
    public bool isBuffed = false;
    [SerializeField] Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DropMoneyBags());
    }
    private void Update()
    {
        slider.value = GetComponent<Health>().health;
    }
    IEnumerator DropMoneyBags()
    {
        GameObject moneyBag = Instantiate(moneyBagPrefab, new Vector3(GetPlayer().transform.position.x
            , GetPlayer().transform.position.y + 10, GetPlayer().transform.position.z)
            , Quaternion.identity);
        moneyBag.GetComponent<MoneyBag>().setData(moneyBagExplosionImpact, moneyBagExplosionRadius, moneyBagExplosionDamage);
        yield return new WaitForSeconds(moneySpawnRate);
        StartCoroutine(DropMoneyBags());
    }
    IEnumerator SpawnArrows()
    {
        if (!CheckGround())
        {
            animator.SetBool("isStock", false);
            DisableAnyAttackState();
            arrowsShot = 0;
            yield break;
        }

        SummonProjectile(0, arrowSpeed, true);
        arrowsShot += 1;
        yield return new WaitForSeconds(arrowSpawnRate);
        if (arrowsShot < numberOfArrows)
        {
            StartCoroutine(SpawnArrows());
        }
        else
        {
            animator.SetBool("isStock", false);
            DisableAnyAttackState();
            arrowsShot = 0;
        }
    }
    public void AttackPlayer()
    {
        GetPlayer().GetComponent<Health>().TakeDamage(meleeDamage);
        DisableAnyAttackState();
    }
    public void ShootArrows()
    {
        animator.SetBool("isStock", true);
        StartCoroutine(SpawnArrows());
    }
}
