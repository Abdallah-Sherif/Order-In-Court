using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveLine : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float minStart;
    [SerializeField] float maxStart;
    [SerializeField] float bombRate;
    [SerializeField] int bombDamage;
    [SerializeField] GameObject bombPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DropBombs());
    }

    IEnumerator DropBombs()
    {
        Vector3 dirVector = endPoint.position - startPoint.position;

        Vector3 point = startPoint.position + Random.Range(0, Vector3.Distance(endPoint.position, startPoint.position)) * dirVector.normalized;

        GameObject moneyBag = Instantiate(bombPrefab, point, Quaternion.identity);
        moneyBag.GetComponent<MoneyBag>().setData(10, 6, bombDamage);
        yield return new WaitForSeconds(bombRate);
        StartCoroutine(DropBombs());
    }
}
