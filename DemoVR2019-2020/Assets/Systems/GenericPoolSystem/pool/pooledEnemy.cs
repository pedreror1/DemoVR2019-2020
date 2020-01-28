using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pooledEnemy : MonoBehaviour
{
    public float lifeSpan = 3f;
    public float timeAlive = 0f;
    public float speed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        timeAlive = 0f;
    }
    public void Dead()
    {
        EnemiesPool.Instance.returnToPool(this);

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
