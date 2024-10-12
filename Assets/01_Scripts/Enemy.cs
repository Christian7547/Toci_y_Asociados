using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 5f;

    [Header("References")]
    public Rigidbody rb;
    public GameObject children;

    Animator childrenAnimator;
    Transform target;

    void Awake()
    {
        childrenAnimator = children.GetComponent<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (Vector3.Distance(transform.position, target.position) < 10)
        {
            if (transform.position.x < target.position.x)
                children.transform.rotation = Quaternion.Euler(-60, -180, 0);
            else
                children.transform.rotation = Quaternion.Euler(60, 0, 0);
            childrenAnimator.SetBool("isMoving", true);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
            childrenAnimator.SetBool("isMoving", false);
    }
}
