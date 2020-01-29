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
        throw new System.NotImplementedException();
    }

    public void Interaction()
    {
        throw new System.NotImplementedException();
    }


    public void Dead()
    {

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
         timeAlive += Time.deltaTime;
        transform.position += Vector3.forward * speed * Time.deltaTime;
        if(timeAlive>= lifeSpan)
        {
           
            Dead();
        }
    }

   
}
