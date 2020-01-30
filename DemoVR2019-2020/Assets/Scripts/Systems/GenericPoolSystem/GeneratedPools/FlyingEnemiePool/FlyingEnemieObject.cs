using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemieObject : MonoBehaviour, IPooledObject
{

    public bool IsDead = false;
    Material mat;
    float deadTime = 0f;
    Transform target;
    [SerializeField]
    float initialSpeed = 2.5f;
    float speed;
    [SerializeField]
    float DeadDistance=0.13f;
     void Update()
    {
       Interaction();
    }
    private void OnEnable()
    {
     born();
    }
    public void born()
    {
        deadTime = 0f;
        speed = initialSpeed;
        mat = GetComponentInChildren<Renderer>().material;
        target = FindObjectOfType<IKVRPlayer>().transform;
        if(!target)
        {
            Destroy(gameObject);
        }
        mat.SetFloat("fadeRate", deadTime);
        IsDead = false;
    }
    public void Interaction()
    {
        transform.LookAt(target);
        transform.position += transform.forward * speed * Time.deltaTime;
        if(Vector3.Distance(transform.position,target.position)<DeadDistance)
        {
            Dead();
        }
        if(IsDead)
        {
            if(deadTime<1f)
            {
                mat.SetFloat("fadeRate", deadTime);
                deadTime += Time.deltaTime;
                speed=0f;

            }
            else
            {
                Dead();
            }
        }
    }
    public void Dead()
    {
        FlyingEnemiePool.Instance.returnToPool(this);
    }
}