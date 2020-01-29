using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
public class SlimeEnemySpawner : MonoBehaviour
{
    [SerializeField]
    int numEnemies = 10;
    [SerializeField]
    float timeBetweenEnemies = 2f;
    // Start is called before the first frame update
    void Start()
    {
        spawnEnemy();
    }

    async void spawnEnemy()
    {
        await Task.Delay(TimeSpan.FromSeconds(timeBetweenEnemies));
        var newEnemy = EnemiesPool.Instance.getFromPool();
        newEnemy.transform.position = transform.position;
        newEnemy.gameObject.SetActive(true);
        numEnemies--;
        if(numEnemies>0)
        {
            spawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
