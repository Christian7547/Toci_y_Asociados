using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]

    float timeToDestroy = 2f;
    public float speed = 5f;
    public bool healthInvocation = false;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        ToForward();
    }

    void ToForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            var enemy = other.GetComponent<IDamageable>();
            enemy.TakeDamage();
        }
    }
}
