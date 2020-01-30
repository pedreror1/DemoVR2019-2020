using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pooledEnemy : MonoBehaviour, IPooledObject
{
    private void OnEnable()
    {
        born();
    }
    public void born()
    {
        timeAlive=0f;
    }

    public void Interaction()
    {
        timeAlive += Time.deltaTime;
        transform.position += Vector3.forward * speed * Time.deltaTime;
        if (timeAlive >= lifeSpan)
        {

            Dead();
        }
    }


    public void Dead()
    {
        SMPlayerPawn playerPawn;
        if(TryGetComponent<SMPlayerPawn>(out playerPawn))
        {
            QuadTreeManager.Instance.removePawn(playerPawn);
        }
        EnemiesPool.Instance.returnToPool(this);

    }
    public float lifeSpan = 3f;
    public float timeAlive = 0f;
    public float speed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        Interaction();
    }

   
}
