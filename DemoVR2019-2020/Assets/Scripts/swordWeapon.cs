using System.Collections;
using System.Collections.Generic;
  using UnityEngine;

public class swordWeapon : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        FlyingEnemieObject collidingFlyingEnemy;
        if(other.TryGetComponent<FlyingEnemieObject>(out collidingFlyingEnemy))
        {
            collidingFlyingEnemy.IsDead = true;
            Debug.Log("Killed Ghost");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
