using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TypeInvocation
{
    Health, Damage
}

public class Invocation : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int hp = 2;
    public TypeInvocation invocation;

    [Header("References")]
    public GameObject invocationPrefab;
    float speed = 3f;

    Transform target;
    Animator childrenAnimator;
    public GameObject children;

    void Awake()
    {
        childrenAnimator = children.GetComponent<Animator>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Movement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(invocation.ToString() == "Health")
            {
                var player = other.GetComponent<Player>();
                player.Healing();
                Destroyer();
            }
            else
            {
                var player = other.GetComponent<Player>();
                player.TakeDamage();
                Destroyer();
            }
        }
    }

    void Movement()
    {
        if (target != null)
        {
            if (transform.position.x < target.position.x)
                children.transform.rotation = Quaternion.Euler(-60, -180, 0);
            else
                children.transform.rotation = Quaternion.Euler(60, 0, 0);
            childrenAnimator.SetBool("isMoving", true);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void Destroyer() => Destroy(gameObject);

    public void TakeDamage()
    {
        Destroyer();
    }
}
