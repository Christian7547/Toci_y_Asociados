using UnityEngine;

public enum TypeEnemy
{
    Wizard, Flying, Rider, Warrior
}

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public TypeEnemy type;
    public float speed = 5f;

    [Header("References")]
    public GameObject children;
    public GameObject invocationPrefab;
    public Transform firePoint;

    bool canAttack = true;
    float timer = 0f;
    float timeBtwInvocation = 3f;
    Animator childrenAnimator;
    Transform target;

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
        CheckTypeEnemy();
    }

    void CheckTypeEnemy()
    {
        if (target != null)
        {
            switch (type)
            {
                case TypeEnemy.Wizard:
                    if (Vector3.Distance(transform.position, target.position) > 10)
                    {
                        MoveTowardsTarget();
                    }
                    else
                    {
                        childrenAnimator.SetBool("isMoving", false);
                        CheckIfCanAttack();
                        CreateInvocation();
                    }
                    break;
                case TypeEnemy.Rider:
                    if (Vector3.Distance(transform.position, target.position) > 0.05f)
                    {
                        MoveTowardsTarget();
                    }
                    else
                    {
                        childrenAnimator.SetBool("isMoving", false);
                        CheckIfCanAttack();
                        Melee();
                    }
                    break;
                case TypeEnemy.Warrior:
                    if (Vector3.Distance(transform.position, target.position) > 2f)
                    {
                        MoveTowardsTarget();
                    }
                    else
                    {
                        childrenAnimator.SetBool("isMoving", false);
                        CheckIfCanAttack();
                        Melee();
                    }
                    speed = 3f;
                    break;
            }
            Rotation();
        }
    }

    void MoveTowardsTarget()
    {
        childrenAnimator.SetBool("isMoving", true);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void Rotation()
    {
        if (transform.position.x < target.position.x)
            children.transform.rotation = Quaternion.Euler(-60, -180, 0);
        else
            children.transform.rotation = Quaternion.Euler(60, 0, 0);
    }

    void CheckIfCanAttack()
    {
        if (!canAttack)
        {
            childrenAnimator.ResetTrigger("attack");
            timer += Time.deltaTime;
            if (timer >= timeBtwInvocation)
            {
                timer = 0f;
                canAttack = true;
            }
        }

    }

    void CreateInvocation()
    {
        if (canAttack)
        {
            childrenAnimator.SetTrigger("attack");
            Instantiate(invocationPrefab, firePoint.position, Quaternion.identity);
            canAttack = false;
        }
    }

    void Melee()
    {
        if (canAttack)
        {
            childrenAnimator.SetTrigger("attack");
            canAttack = false;
        }
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
