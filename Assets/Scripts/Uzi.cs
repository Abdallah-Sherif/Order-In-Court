using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Uzi : MonoBehaviour
{
    [Header("Bullet Properties")]
    [SerializeField] Transform _shootPos;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletSpread;
    [SerializeField] float _bulletFireRate;
    [SerializeField] KeyCode _shootKey;
    [SerializeField] GameObject _bulletPrefab;
    bool canFire = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(_shootKey))
        {
            ShootBullets();
        }
    }
    void ShootBullets()
    {
        if(canFire) 
        {
            StartCoroutine(CoolDown());
            float x_offset = UnityEngine.Random.Range(-_bulletSpread, _bulletSpread);
            float y_offset = UnityEngine.Random.Range(-_bulletSpread, _bulletSpread);
            Vector3 dir = transform.forward + new Vector3(x_offset, y_offset, 0);
            dir.Normalize();
            GameObject bullet = Instantiate(_bulletPrefab, _shootPos.position, Quaternion.LookRotation(dir) );
            bullet.GetComponent<Rigidbody>().velocity = dir * _bulletSpeed;
            bullet.tag = this.transform.tag;
        }
    }
    IEnumerator CoolDown()
    {
        canFire = false;
        yield return new WaitForSeconds(_bulletFireRate);
        canFire = true;
    }
}
