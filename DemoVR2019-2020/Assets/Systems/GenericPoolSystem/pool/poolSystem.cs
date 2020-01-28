using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class poolSystem<T>:SingletonComponent<poolSystem<T>> where T: Component
{
    [SerializeField]
    private T prefab;

    private Queue<T> objects = new Queue<T>();
    


   
    public T getFromPool()
    {
        if(objects.Count==0)
        {
            AddObjects(1);
        }

        return objects.Dequeue();
    }

    private void AddObjects(int v)
    {
        for (int i = 0; i < v; i++)
        {
            var newObj = GameObject.Instantiate(prefab);
            newObj.gameObject.SetActive(false);
            objects.Enqueue(newObj);
        }
    }

    public void returnToPool(T poolObject)
    {
        poolObject.gameObject.SetActive(false);
        objects.Enqueue(poolObject);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
