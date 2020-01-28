using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] float Speed = 1f;
    [SerializeField] float lifeTime= 3f;
    float AliveTime = 3f;
    float sizeDelta = 0f;
    [SerializeField] float scale = 5f;

    private void OnEnable()
    {
        AliveTime = lifeTime;
        sizeDelta = 0f;
        transform.localScale = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one*scale, sizeDelta);
        transform.position += transform.forward * Speed*Time.deltaTime;
        AliveTime -= Time.deltaTime;
        sizeDelta += (sizeDelta<1f? Time.deltaTime:0);
        if(AliveTime<=0f)
        {
            MonsterBulletPool.Instance.returnToPool(this);
        }
    }
}
