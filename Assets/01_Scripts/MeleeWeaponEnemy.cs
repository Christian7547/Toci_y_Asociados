using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.TakeDamage();
        }
    }
}
