using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool toRight = true;

    public void Shoot()
    {
        if(toRight)
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 90, 0));
        else
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, -90, 0));
    }
}
