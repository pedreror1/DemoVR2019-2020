using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    int numEnemies = 10;
    [SerializeField]
    float timeBetweenEnemies = 2f;

    public enum EnemyType
    {
        flyingEnemy,
        slimeEnemy
    }
    public EnemyType spawnerEnemyType= EnemyType.slimeEnemy;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemyco());
    }
    IEnumerator spawnEnemyco()
    {
        yield return new WaitForSeconds(timeBetweenEnemies);
        spawnEnemy();
    }
    void spawnEnemy()
    {
       // await Task.Delay(TimeSpan.FromSeconds(timeBetweenEnemies));
        if(EnemiesPool.Instance)
        {
            GameObject newEnemy = null;
            switch(spawnerEnemyType)
            {
                case EnemyType.slimeEnemy:

                newEnemy = EnemiesPool.Instance.getFromPool().gameObject;
                break;

                case EnemyType.flyingEnemy:
                newEnemy = FlyingEnemiePool.Instance.getFromPool().gameObject;
                break;
            }
            if(newEnemy)
            {
                newEnemy.transform.parent= transform;
                newEnemy.transform.position = transform.position;
                newEnemy.gameObject.SetActive(true);
                newEnemy.name = "Slime" + Time.time;
                numEnemies--;
                if(numEnemies>0)
                {
                   StartCoroutine(spawnEnemyco());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
