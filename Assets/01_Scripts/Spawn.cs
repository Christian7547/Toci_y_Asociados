using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [Header("Enemies")]
    public List<GameObject> enemies;
    int maxEnemiesToSpawn;
    int currentEnemiesSpawned = 0;

    float timer = 0f;
    float timeBtwSpawn = 2f;

    void Start()
    {
        maxEnemiesToSpawn = Random.Range(5, 8);
    }

    void Update()
    {
        CheckIfCanSpawn();
    }

    void CheckIfCanSpawn()
    {
        timer += Time.deltaTime;
        if (timer >= timeBtwSpawn)
        {
            timer = 0;
            if (currentEnemiesSpawned <= maxEnemiesToSpawn)
            {
                Spawner();
                currentEnemiesSpawned += 1;
            }
        }
    }

    void Spawner()
    {
        int randomIndex = Random.Range(0, enemies.Count);
        var randomEnemy = enemies[randomIndex];
        Instantiate(randomEnemy, transform.position, Quaternion.identity);
    }
}
