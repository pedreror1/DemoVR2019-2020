using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour,IPooledObject
{
    [SerializeField] float Speed = 1f;
    [SerializeField] float lifeTime= 3f;
    float AliveTime = 3f;
    float sizeDelta = 0f;
    [SerializeField] float scale = 5f;

    public void born()
    {
        gameObject.SetActive(true);
        AliveTime = lifeTime;
        sizeDelta = 0f;
        transform.localScale = Vector3.zero;
    }

    public void Dead()
    {
        MonsterBulletPool.Instance.returnToPool(this);
    }

    public void Interaction()
    {
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * scale, sizeDelta);
        transform.position += transform.forward * Speed * Time.deltaTime;
        AliveTime -= Time.deltaTime;
        sizeDelta += (sizeDelta < 1f ? Time.deltaTime : 0);
    }

    private void OnEnable()
    {
        born();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Interaction();
        if(AliveTime<=0f)
        {
            Dead();
        }
    }
}
