using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public bool dead = false;
    Material mat;
    float deadTime = 0f;
    Transform target;
    float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        deadTime = 0f;
        mat = GetComponentInChildren<Renderer>().material;
        target = FindObjectOfType<monavr>().transform;
        if(!target)
        {
            Destroy(gameObject);
        }
        mat.SetFloat("fadeRate", deadTime);
        dead = false;
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        transform.position += transform.forward * speed * Time.deltaTime;
        if(dead)
        {
            if(deadTime<1f)
            {
                mat.SetFloat("fadeRate", deadTime);
                deadTime += Time.deltaTime;
            }
            else
            {
                Dead();
            }
        }
    }
}
